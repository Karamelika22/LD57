﻿using System.Collections;
using System.Linq;
using UnityEngine;
using FMODUnity; // Добавляем пространство имен FMOD

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ClickMoveController : MonoBehaviour
{
    [Header("Настройки передвижения")]
    public float speed = 5f;

    [Header("Настройки клика по объекту")]
    public float clickRadius = 0.5f; // радиус, в котором клик считается по объекту

    // Добавляем настройки FMOD
    [Header("Настройки звука")]
    [SerializeField] private EventReference footstepsSound; // Ссылка на событие FMOD
    [SerializeField] private EventReference characterClickSound;
    private FMOD.Studio.EventInstance footstepsInstance; // Экземпляр события
    private bool isMoving = false; // Флаг движения для управления звуком

    private bool isAwaitingTarget = false;
    private Coroutine moveCoroutine;

    // Кэш всех зон на сцене
    [SerializeField] private PolygonCollider2D[] movementZones;

    private Rigidbody2D rb;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private LayerMask obstacleMask;

    private PlayerAnim playerAnim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        playerAnim = GetComponent<PlayerAnim>();

        // Инициализируем экземпляр звука FMOD
        footstepsInstance = RuntimeManager.CreateInstance(footstepsSound);
        footstepsInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void Update()
    {
        // Обработка клика по объекту (если ещё не в режиме выбора точки)
        if (!isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // Проверяем, попал ли клик в область вокруг объекта
            if ((clickWorld - transform.position).sqrMagnitude <= clickRadius * clickRadius)
            {
                RuntimeManager.PlayOneShot(characterClickSound, transform.position);

                isAwaitingTarget = true;
                Debug.Log("Клик по игроку — выберите точку назначения");
                return; // чтобы в тот же кадр не обработать выбор точки
            }
        }

        // Если режим выбора точки включён — слушаем второй клик
        if (isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // Проверяем, лежит ли точка внутри любой из зон
            bool insideAny = movementZones.Any(zone =>
            {
                // OverlapPoint работает только в режиме 2D, т.е. z-координата игнорируется
                return zone.OverlapPoint((Vector2)clickWorld);
            });

            if (insideAny)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveTo(clickWorld));
            }
            else
            {
                Debug.Log("Точка за пределами допустимой зоны");
            }

            isAwaitingTarget = false;
        }
    }

    private IEnumerator MoveTo(Vector3 dest)
    {
        // Начинаем звук шагов
        StartFootstepsSound();

        if (playerAnim != null)
        {
            playerAnim.PlayWalkAnimation(true);
        }

        while (true)
        {
            Vector3 newPos = Vector3.MoveTowards(
                transform.position,
                dest,
                speed * Time.deltaTime
            );

            // Проверка коллизий только при необходимости
            if (Physics2D.OverlapPoint(newPos, obstacleMask))
            {
                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }
                // Останавливаем звук при столкновении
                StopFootstepsSound();
                Debug.Log("Обнаружено препятствие!");
                yield break;
            }

            transform.position = newPos;

            if ((transform.position - dest).sqrMagnitude < stoppingDistance)
            {
                transform.position = dest;
                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }
                // Останавливаем звук при достижении цели
                StopFootstepsSound();
                Debug.Log("Достигнута конечная точка");
                yield break;
            }

            yield return null;
        }
    }

    // Новые методы для управления звуком шагов
    private void StartFootstepsSound()
    {
        if (!isMoving)
        {
            footstepsInstance.start();
            isMoving = true;
        }
    }

    private void StopFootstepsSound()
    {
        if (isMoving)
        {
            footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }
    }

    void OnDestroy()
    {
        // Освобождаем ресурсы FMOD при уничтожении объекта
        footstepsInstance.release();
    }

    // Для наглядности можно в OnDrawGizmos отрисовать зону передвижения и зону клика:
    void OnDrawGizmosSelected()
    {
        // Радиус клика по объекту
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}
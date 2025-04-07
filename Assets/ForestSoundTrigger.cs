using UnityEngine;
using FMODUnity;

public class ForestSoundTrigger : MonoBehaviour
{
    private StudioEventEmitter emitter;

    void Start()
    {
        // Получаем компонент FMOD Studio Event Emitter с этого объекта
        emitter = GetComponent<StudioEventEmitter>();
    }

    // Вызывается, когда объект входит в зону триггера
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что это игрок (по тегу, слою или имени)
        if (other.CompareTag("Player")) // Убедись, что у игрока есть тег "Player"
        {
            if (emitter != null && !emitter.IsPlaying())
            {
                emitter.Play(); // Включаем звук
            }
        }
    }

    // Вызывается, когда объект покидает зону триггера
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (emitter != null && emitter.IsPlaying())
            {
                emitter.Stop(); // Останавливаем звук
            }
        }
    }
}
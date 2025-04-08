using System.Collections;
using UnityEngine;

public class PlayerTeleporterHandler : MonoBehaviour
{
    public float teleportDelay = 0.5f;

    private Teleporter currentTeleporter;
    private bool canTeleport = false;
    private bool isTeleporting = false;

    void Update()
    {
        if (canTeleport && !isTeleporting && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(TeleportWithDelay());
        }
    }

    private IEnumerator TeleportWithDelay()
    {
        isTeleporting = true;
        Debug.Log("Телепортация через " + teleportDelay + " сек...");
        yield return new WaitForSeconds(teleportDelay);

        if (currentTeleporter != null && currentTeleporter.destination != null)
        {
            transform.position = currentTeleporter.destination.position;
            Debug.Log("Игрок телепортирован в зону: " + currentTeleporter.zoneTo.zoneName);
        }

        isTeleporting = false;
        canTeleport = false;
        currentTeleporter = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            currentTeleporter = teleporter;
            canTeleport = true;
            Debug.Log("Вход в зону телепорта. Нажмите F, чтобы переместиться.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null && teleporter == currentTeleporter)
        {
            canTeleport = false;
            currentTeleporter = null;
            Debug.Log("Выход из зоны телепорта.");
        }
    }
}

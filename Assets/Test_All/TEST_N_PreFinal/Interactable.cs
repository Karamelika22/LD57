using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionType; // "Light", "Mineral", "Stick", "Crafting"
    [SerializeField] private GameObject interactionHint; // ��������� "Press [F]"
    [Header("Pile Settings")]
    [SerializeField] private bool isPile = false;
    
    private bool _isPlayerInRange = false; // ��������� ���� ��� ������������ ������ � ����
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InteractManager.Instance.SetPlayerInRange(true);
        _isPlayerInRange = true; // ������������� ���� ��� ����� �������
        if(interactionHint!=null) interactionHint.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InteractManager.Instance.SetPlayerInRange(false);
        _isPlayerInRange = false; // ���������� ����
        if (interactionHint != null) interactionHint.SetActive(false);
        if (InteractManager.Instance != null) InteractManager.Instance.ForceCloseCraftingUI();
    }

    private void Update()
    {
        // ��������� ��������� ���� ������ �����������
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F) && InteractManager.Instance != null)
        {
            if (isPile)
            {
                InteractManager.Instance.HandleInteraction(interactionType);
                StartCoroutine(DestroyPileWithDelay(5.01f));
            }
            else
            {
                InteractManager.Instance.HandleInteraction(interactionType);
            }
        }
    }
    private IEnumerator DestroyPileWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    public void EndGame()
    {
        interactionHint.SetActive(false);
    } 

}

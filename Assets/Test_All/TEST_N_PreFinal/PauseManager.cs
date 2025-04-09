using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public Image pauseScrean;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
       
        if (isPaused)
        {
            pauseScrean.gameObject.SetActive(true);
            Time.timeScale = 0f; 
        }
        else
        {
            pauseScrean.gameObject.SetActive(false);
            Time.timeScale = 1f; 
            Debug.Log("Пауза выключена");
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
    }
    public bool IsPaused()
    {
        return isPaused;
    }
}
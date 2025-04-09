using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMMM : MonoBehaviour
{
    public void SelectScene(int sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId); // 0 - menu; 1 - firstArea; 
    }

}

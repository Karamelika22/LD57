using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMMM : MonoBehaviour
{
    public void SelectScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId); // 0 - menu; 1 - firstArea; 
    }
}

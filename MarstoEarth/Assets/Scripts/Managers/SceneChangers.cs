using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangers : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

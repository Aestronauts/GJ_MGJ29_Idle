using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void SceneLoader(string _sceneToLoad) // NOTE - May want to wait for animation timeline to finish first
    {
        Debug.Log("Loading Scene");
        Time.timeScale = 1;
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene(_sceneToLoad);
    }
}

using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public string CurrentActiveMainScene { get; private set; }

    public bool LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        CurrentActiveMainScene = sceneName;
        return true;
    }

    public bool LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        CurrentActiveMainScene = sceneName;
        return true;
    }
}

using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.Instance.LoadSceneAdditive(SceneNames.SceneGlobalUI);
            SceneManager.Instance.LoadSceneAndUnloadCurrent(SceneNames.SceneMainMenu, false);
        }
    }
}

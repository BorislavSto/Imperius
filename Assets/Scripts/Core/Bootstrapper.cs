using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.Instance.LoadScenesAdditive(new[] { SceneNames.SceneGlobalUI,  SceneNames.SceneLoading }); 
            SceneManager.Instance.LoadSceneAndUnloadCurrent(SceneNames.SceneMainMenu, false);
        }
    }
}

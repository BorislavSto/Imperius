using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private string[] baseScenes;

        private void Start()
        {
            foreach (var scene in baseScenes)
                SceneManager.Instance.LoadSceneAdditive(scene);

            SceneManager.Instance.LoadSceneAndUnloadCurrent(sceneName, false);
        }
    }
}

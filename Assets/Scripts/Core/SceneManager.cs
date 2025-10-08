using System.Collections;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneManager : Singleton<SceneManager>
    {
        private string currentActiveMainScene = "Bootstrap";

        public void LoadSceneAndUnloadCurrent(string sceneName, bool waitForInput)
        {
            StartCoroutine(LoadSceneAndUnloadCurrentCoroutine(sceneName, waitForInput));
        }

        public void LoadSceneAdditive(string sceneName)
        {
            StartCoroutine(LoadSceneAdditiveCoroutine(sceneName));
        }

        private IEnumerator LoadSceneAdditiveCoroutine(string sceneName)
        {
            AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (loadOp == null)
                yield break;
                
            while (!loadOp.isDone)
                yield return null;
        }
        
        /// <summary>
        /// Coroutine to load a new scene while unloading the current active scene.
        /// Optionally waits for user input before activating the new scene.
        /// </summary>
        /// <param name="newScene">The name of the new scene to load.</param>
        /// <param name="waitForInput">If true, waits for user input before activating the new scene.</param>
        private IEnumerator LoadSceneAndUnloadCurrentCoroutine(string newScene, bool waitForInput)
        {
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SceneLoading", LoadSceneMode.Additive);
            
            if (!string.IsNullOrEmpty(currentActiveMainScene))
            {
                Debug.Log($"Unloading {currentActiveMainScene}");
                AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentActiveMainScene);
                if (unloadOp != null)
                {
                    while (!unloadOp.isDone)
                        yield return null;
                }
            }

            AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            if (loadOp != null)
            {
                if (waitForInput)
                    loadOp.allowSceneActivation = false;
                
                while (loadOp.progress < 0.9f)
                {
                    // update a loading bar
                    Debug.Log($"Loading {newScene}: {loadOp.progress * 100}%");
                    yield return null;
                }

                if (waitForInput)
                {
                    // make a "Press any key to continue" prompt
                    Debug.Log("Scene ready! Press any key to continue...");

                    bool pressed = false;

                    var binding = new EventBinding<AnyButtonPressed>(_ => pressed = true);
                    EventBus<AnyButtonPressed>.Register(binding);

                    yield return new WaitUntil(() => pressed);

                    EventBus<AnyButtonPressed>.Deregister(binding);

                    loadOp.allowSceneActivation = true;
                }

                while (!loadOp.isDone)
                    yield return null;
            }
            
            yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("SceneLoading");

            currentActiveMainScene = newScene;
        }
    }
}

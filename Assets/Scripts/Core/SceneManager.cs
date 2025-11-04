using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneManager : Singleton<SceneManager>
    {
        private List<string> currentActiveMainScenes = new() { SceneNames.SceneBootstrap };
        
        /// <summary>
        /// This method is only used to load additive scenes that do not replace the current active scene.
        /// Used for scenes such as "SceneGlobalUI" or "SceneAudio".
        /// </summary>
        public void LoadScenesAdditive(string[] sceneNames)
        {
            StartCoroutine(LoadScenesAdditiveCoroutine(sceneNames));
        }

        public void LoadSceneAndUnloadCurrent(string sceneName, bool waitForInput)
        {
            StartCoroutine(LoadSceneAndUnloadCurrentCoroutine(sceneName, waitForInput));
        }

        public void LoadMultipleScenesAndUnloadCurrent(string[] scenesNames, bool waitForInput)
        {
            StartCoroutine(LoadMultipleScenesAndUnloadCurrentCoroutine(scenesNames, waitForInput));
        }

        private IEnumerator LoadScenesAdditiveCoroutine(string[] sceneNames)
        {
            foreach (string sceneName in sceneNames)
            {
                AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                if (loadOp == null)
                    yield break;

                while (!loadOp.isDone)
                    yield return null;
            }
        }

        /// <summary>
        /// Coroutine to load a new scene while unloading the current active scene.
        /// Optionally waits for user input before activating the new scene.
        /// </summary>
        /// <param name="newScene">The name of the new scene to load.</param>
        /// <param name="waitForInput">If true, waits for user input before activating the new scene.</param>
        private IEnumerator LoadSceneAndUnloadCurrentCoroutine(string newScene, bool waitForInput)
        {
            EventBus<SceneLoadingEvent>.Raise(new SceneLoadingEvent(true));
            
            string[] oldScenes = currentActiveMainScenes.ToArray();
            currentActiveMainScenes.Clear();
            
            AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            if (loadOp != null)
            {
                if (waitForInput)
                    loadOp.allowSceneActivation = false;

                while (loadOp.progress < 0.9f)
                {
                    float loadingPercentage = loadOp.progress / 0.9f;
                    EventBus<SceneLoadingProgress>.Raise(new SceneLoadingProgress(loadingPercentage));
                    yield return null;
                }

                if (waitForInput)
                {
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

            foreach (string scene in oldScenes)
            {
                if (!string.IsNullOrEmpty(scene))
                {
                    Debug.Log($"Unloading {scene}");
                    AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                    if (unloadOp != null)
                    {
                        while (!unloadOp.isDone)
                            yield return null;
                    }
                }
            }
            
            EventBus<SceneLoadingEvent>.Raise(new SceneLoadingEvent(false));

            currentActiveMainScenes.Add(newScene);
        }

        /// <summary>
        /// Coroutine to load multiple new scenes while unloading the current active scenes.
        /// Optionally waits for user input before activating the last new scene.
        /// </summary>
        /// <param name="newScenes"></param>
        /// <param name="waitForInput"></param>
        /// <returns></returns>
        private IEnumerator LoadMultipleScenesAndUnloadCurrentCoroutine(string[] newScenes, bool waitForInput)
        {
            EventBus<SceneLoadingEvent>.Raise(new SceneLoadingEvent(true));
            
            foreach (string scene in currentActiveMainScenes)
            {
                if (!string.IsNullOrEmpty(scene))
                {
                    Debug.Log($"Unloading {scene}");
                    AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                    if (unloadOp != null)
                    {
                        while (!unloadOp.isDone)
                            yield return null;
                    }
                }
            }
            
            currentActiveMainScenes.Clear();

            for (int i = 0; i < newScenes.Length; i++)
            {
                string newScene = newScenes[i];
                bool isLastScene = i == newScenes.Length - 1;

                AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
                if (loadOp != null)
                {
                    if (waitForInput && isLastScene)
                        loadOp.allowSceneActivation = false;

                    while (loadOp.progress < 0.9f)
                    {
                        float loadingPercentage = loadOp.progress / 0.9f;
                        EventBus<SceneLoadingProgress>.Raise(new SceneLoadingProgress(loadingPercentage));
                        yield return null;
                    }
                    
                    EventBus<SceneLoadingProgress>.Raise(new SceneLoadingProgress(1f));

                    if (waitForInput && isLastScene)
                    {
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

                if (isLastScene)
                    EventBus<SceneLoadingEvent>.Raise(new SceneLoadingEvent(false));

                currentActiveMainScenes.Add(newScene);
            }
        }
    }
}

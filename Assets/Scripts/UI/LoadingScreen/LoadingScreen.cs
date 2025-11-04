using System;
using EventBus;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBackground;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TMP_Text loadingText;
    
    [SerializeField] private LoadingScreenData loadingData;
    
    private EventBinding<SceneLoadingProgress> loadingProgressBinding;
    
    // private string[] arrayOfLoadingTexts; could have multiple rotating loading texts

    private void Awake()
    {
        UIManager.Instance.TriggerLoading += TriggerLoading;
        
        loadingProgressBinding = new EventBinding<SceneLoadingProgress>(LoadingProgress);
        EventBus<SceneLoadingProgress>.Register(loadingProgressBinding);
        
        HideLoadingScreen();    
    }

    private void OnDestroy()
    {
        UIManager.Instance.TriggerLoading -= TriggerLoading;
        
        EventBus<SceneLoadingProgress>.Deregister(loadingProgressBinding);
    }

    private void TriggerLoading(bool isLoading)
    {
        if (isLoading)
            ShowLoadingScreen(loadingData);
        else
            HideLoadingScreen();
    }

    private void ShowLoadingScreen(LoadingScreenData data)
    {
        loadingScreen.SetActive(true);
        loadingBackground.sprite = data.loadingSprite;
        loadingText.text = data.loadingText;
    }

    private void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
        loadingBackground.sprite = null;
        loadingText.text = "";
        loadingBar.fillAmount = 0;
    }
    
    private void LoadingProgress(SceneLoadingProgress loadingProgress)
    {
        if (Mathf.Approximately(loadingProgress.LoadingProgress, 1f))
            loadingText.text = "Press Any Button To Continue...";
        
        loadingBar.fillAmount = loadingProgress.LoadingProgress;
    }
}

[Serializable]
public struct LoadingScreenData
{
    public Sprite loadingSprite;
    public string loadingText;
}

using UnityEngine;

namespace UI
{
    public class GameplayUISystem : MvvmSystem<GameplayUIViewModel>
    {
        public static GameplayUISystem Instance => instance;
        private static GameplayUISystem instance;
        
        [SerializeField] private GameplayUIView gameplayUIView;
        
        // exception for other singletons, has to be an MvvmSystem while being a singleton
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            
            viewModel = new GameplayUIViewModel(gameplayUIView, new GameplayUIModel());
        }
    }
}
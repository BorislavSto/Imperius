using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;
        //[SerializeField] private 

        private MainMenuViewModel viewModel;

        public override void Init(ViewModel vm)
        {
            viewModel = vm as MainMenuViewModel;

            optionsButton.onClick.AddListener(() => viewModel.OnOptionsClicked());
            startButton.onClick.AddListener(() => viewModel.OnStartClicked());
            quitButton.onClick.AddListener(() => viewModel.OnQuitClicked());
        }

        public override void Show()
        {
            base.Show();
            Debug.Log("Main Menu Shown");
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("Main Menu Hidden");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayUIView : View
    {
        private GameplayUIViewModel viewModel;

        [SerializeField] private Image background;

        // here i can do things such as change the textures, health etc

        public void ShowSettings() => Show();
        public void HideSettings() => Hide();
    }
}

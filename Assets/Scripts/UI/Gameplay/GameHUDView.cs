using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameHUDView : View
    {
        private GameHUDViewModel viewModel;

        [SerializeField] private Image background;

        // here i can do things such as change the textures, health etc

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}

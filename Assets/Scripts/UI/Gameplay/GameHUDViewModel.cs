namespace UI
{
    public class GameHUDViewModel : ViewModel
    {
        private GameHUDView view;
        private GameHUDModel model;
        public GameHUDViewModel(GameHUDView view, GameHUDModel model) : base(view, model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
        }
    }
}
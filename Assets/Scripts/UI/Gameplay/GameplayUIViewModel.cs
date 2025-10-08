namespace UI
{
    public class GameplayUIViewModel : ViewModel
    {
        private GameplayUIView view;
        private GameplayUIModel model;
        
        public GameplayUIViewModel(GameplayUIView view, GameplayUIModel model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
        }
    }
}
using UnityEngine;

namespace UI
{
    public class Model
    {
        protected Model()
        {
        }
    }

    public class View : MonoBehaviour
    {
        public virtual void Init(ViewModel viewModel) { }
        public virtual void Show() { }
        public virtual void Hide() { }
    }

    public class ViewModel
    {
        public View View { get; private set; }
        public Model Model { get; private set; }

        protected ViewModel(View view, Model model)
        {
            View = view;
            Model = model;
        }
    }
}

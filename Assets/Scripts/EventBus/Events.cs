namespace EventBus
{
    public interface IEvent {}
    public struct EscapeButtonPressed : IEvent {}
    public struct AnyButtonPressed : IEvent {}

    public struct SceneLoadingProgress : IEvent
    {
        public readonly float LoadingProgress;
        
       public SceneLoadingProgress(float loadingProgress)
        {
            LoadingProgress = loadingProgress;
        }
    }

    public struct SceneLoadingEvent : IEvent
    {
        public readonly bool IsLoading;
        public SceneLoadingEvent(bool isLoading)
        {
            this.IsLoading = isLoading;
        }
    }
}


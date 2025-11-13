namespace Core
{
    public static class GlobalData
    {
        public static bool GameLoading { get; private set; }
        public static void SetGameLoading(bool value) => GameLoading = value;
    }
}

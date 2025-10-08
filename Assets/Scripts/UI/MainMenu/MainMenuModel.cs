using System.Collections.Generic;

namespace UI
{
    public class MainMenuModel : Model
    {
        // Model will keep information after being "read" by the ViewModel
        // such as character information, levels, settings etc.
        // example information **
        public string PlayerName { get; set; }
        public int CurrentLevel { get; set; }
        public List<string> UnlockedLevels { get; set; } = new ();
        public float MasterVolume { get; set; }
        public bool IsFullscreen { get; set; }
        public string SelectedOption { get; set; } = "StartGame";
        
        public override void Save() {}
        public override void Load() {}
    }
}

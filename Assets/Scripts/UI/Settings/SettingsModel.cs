using UnityEngine;

namespace UI
{
    public class SettingsModel : Model
    {
        // Audio
        public float Volume { get; set; } = 1f;
        public bool Muted { get; set; } = false;

        // Gameplay
        public bool ShowHints { get; set; } = true;

        // Graphics
        public bool Fullscreen { get; set; } = true;
        public int ResolutionIndex { get; set; } = 0;
        
        private const string SaveKey = "PlayerSettings";

        public override void Save()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public override void Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                //Debug.Log("No saved settings, using defaults.");
            }
        }
    }
}
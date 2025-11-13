using UnityEngine;

namespace UI
{
    public class SettingsModel : Model
    {
        // Audio
        public float volume { get; set; } = 1f;
        public bool muted { get; set; }

        // Gameplay
        public bool showHints { get; set; } = true;

        // Graphics
        public bool fullscreen { get; set; } = true;
        public int resolutionIndex { get; set; }
        
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
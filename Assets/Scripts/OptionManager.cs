using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {
    const int DEFAULT_DIFFICULTY_INDEX = 1;
    const float DEFAULT_VOLUME = 50;
    const bool DEFAULT_MUSIC_ENABLE = true;

    public ToggleGroup difficultyToggleGroup;
    public Text difficultyDescription;
    public Slider volumeSlider;
    public Toggle musicMuteCheck;

    private MusicManager musicManager;
    private Toggle[] toggles;
    private string[] descriptions;

    private void Awake() {
        musicManager = GameObject.FindObjectOfType<MusicManager>();
        toggles = difficultyToggleGroup.GetComponentsInChildren<Toggle>();
        descriptions = new string[3];

        if (!musicManager) Debug.LogError("Music manager not found!");
    }

    private void Start() {
        toggles[PlayerPrefManager.GetDiffculty()].isOn = true;
        volumeSlider.value = PlayerPrefManager.GetMasterVolume();
        musicMuteCheck.isOn = PlayerPrefManager.isMusicEnabled();

        descriptions[0] = "This is the easy mode";

        descriptions[1] = "This is the normal mode";

        descriptions[2] = "This is the extreme mode";
    }

    private void Update() {
        for(int i = 0; i < toggles.Length; i++) {
            if(toggles[i].isOn) {
                difficultyDescription.text = descriptions[i];
                break;
            }
        }
    }

    public void Save() {
        for (int i = 0; i < toggles.Length; i++) {
            if (toggles[i].isOn) {
                PlayerPrefManager.SetDifficulty(i);
                break;
            }
        }

        PlayerPrefManager.SetMasterVolume(volumeSlider.value);
        musicManager.GetAudioSource().volume = volumeSlider.value;

        PlayerPrefManager.EnableMusic(musicMuteCheck.isOn);
        musicManager.GetAudioSource().mute = !musicMuteCheck.isOn;
    }

    public void SetDefault() {
        toggles[DEFAULT_DIFFICULTY_INDEX].isOn = true;
        volumeSlider.value = DEFAULT_VOLUME;
        musicMuteCheck.isOn = DEFAULT_MUSIC_ENABLE;
    }
}

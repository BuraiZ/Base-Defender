using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefManager : MonoBehaviour {
    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string MUSIC_ENABLE_KEY = "music_enable";

    public static void SetMasterVolume(float volume) {
        if(volume >= 0f && volume <= 100f) {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        } else {
            Debug.LogError("Master Volume out of range!");
        }
    }

    public static float GetMasterVolume() {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void SetDifficulty(int difficulty) {
        if (difficulty >= 0 && difficulty <= 2) {
            PlayerPrefs.SetInt(DIFFICULTY_KEY, difficulty);
        } else {
            Debug.LogError("difficulty out of range!");
        }
    }

    public static int GetDiffculty() {
        return PlayerPrefs.GetInt(DIFFICULTY_KEY);
    }

    public static void EnableMusic(bool isEnabled) {
        if (isEnabled) {
            PlayerPrefs.SetInt(MUSIC_ENABLE_KEY, 1);  //use 1 for true and 0 for false
        } else {
            PlayerPrefs.SetInt(MUSIC_ENABLE_KEY, 0);
        }
    }

    public static bool isMusicEnabled() {
        int enabled = PlayerPrefs.GetInt(MUSIC_ENABLE_KEY);
        return enabled == 1;
    }
}

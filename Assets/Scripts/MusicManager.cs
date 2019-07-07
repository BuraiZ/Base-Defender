using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    public AudioClip splashClip;
    public AudioClip menuClip;
	public AudioClip[] levelClips;
	private AudioSource audioSource;
    private int startLevelIndex;

	void Awake() {
		DontDestroyOnLoad (gameObject);
        audioSource = GetComponent<AudioSource>();
        startLevelIndex = 5;
    }

    public AudioSource GetAudioSource() {
        return audioSource;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLoadLevel;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLoadLevel;
    }

    //Called at the start of new scene
    private void OnLoadLevel(Scene scene, LoadSceneMode loadSceneMode) {
        AudioClip clipToPLay = null;

        if (scene.name == "Splash") clipToPLay = splashClip;
        else if (scene.name == "Menu") clipToPLay = menuClip;
        else {
            if (scene.buildIndex >= startLevelIndex) {
                if (scene.buildIndex - startLevelIndex + 1 <= levelClips.Length) {
                    AudioClip currentClip = levelClips[scene.buildIndex - startLevelIndex];
                    if (currentClip) {
                        clipToPLay = currentClip;
                    }
                } else {
                    print("there is no audio clip associated with this scene!");
                }
            }
        }

        if (clipToPLay != null && audioSource.clip != clipToPLay) PlayClip(clipToPLay);     //don't play the repeat the same music on scene change
    }

    private void PlayClip(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}

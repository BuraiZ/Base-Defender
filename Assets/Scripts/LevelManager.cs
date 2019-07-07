using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	private const int SPLASH_DELAY = 6;		//seconds
    private static int previousLevelIndex;
    public static LevelManager _instance;
    public bool dontDestroyOnLoad;

    void Start() {
        if (dontDestroyOnLoad) {
            if (_instance != null) {
                Destroy(gameObject);
            } else {
                _instance = this;
                GameObject.DontDestroyOnLoad(gameObject);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 0) Invoke ("LoadStartMenu", SPLASH_DELAY);  //build index 0 --> splash scene
    }

	public void LoadStartMenu() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene ("Menu");
    }

	public void StartGame() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Level01");
    }

    public void LoadOption() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Option");
    }

    public void LoadControl() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Control");
    }

    public void LoadInstruction() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Instruction");
    }

    public void LoadNextLevel() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousLevel() {
        SceneManager.LoadScene(previousLevelIndex);
    }

    public void LoadEndGameResult() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("ResultBoard");
    }

    public void LoadWinnerScreen() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Winner");
    }

	public void LoadLoserScreen() {
        previousLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Loser");
    }

	public void Quit() {
		Application.Quit();
	}
}

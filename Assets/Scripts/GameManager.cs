using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public GameObject playerPrefab;
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public ScoreBoard scoreBoard;

    private LevelManager levelManager;
    private NetworkManager networkManager;
    private Health baseHealth;
    private bool isPaused;

    private void Awake() {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        networkManager = GameObject.FindObjectOfType<NetworkManager>();

        if (!levelManager) Debug.LogError("Level manager not found!");
        if (!networkManager) Debug.LogError("Network manager not found!");
    }

    private void Start() {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
    }

    public override void OnStartServer() {
        baseHealth = GameObject.Find("Base").GetComponent<Health>();
    }

    private void Update() {
        if (!baseHealth) return;
        
        VerifyLoseCondition();
        HandleShorcutInput();

        if (Input.GetKeyDown("space")) {
            GameOver();
        }
    }

    public GameObject GetLocalPLayer() {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players) {
            if (player.isLocalPlayer) return player.gameObject;
        }
        return null;
    }

    private void VerifyLoseCondition() {
        if (baseHealth.healthPoint <= 0) {
            GameOver();
        }
    }

    private void GameOver() {
        scoreBoard.RpcGetEndGameResult();
        networkManager.ServerChangeScene("ResultBoard");
    }

    private void HandleShorcutInput() {
        if (Input.GetKeyDown("p")) {   //pause shortcut
            DisplayPauseMenu();
        }
    }

    public void DisplayPauseMenu() {
        isPaused = !isPaused;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;

        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void OpenOptionMenu() {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void CloseOptionMenu() {
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InstructionMenu : MonoBehaviour {
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject menuButton;

    public GameObject missionPage;
    public GameObject databasePage;
    public GameObject dataPage;
    
    private int currentPageIndex;
    private bool isInDataView;
    private Dictionary<int, GameObject> pages;

    private void Awake() {
        pages = new Dictionary<int, GameObject>();
    }

    private void Start() {
        currentPageIndex = 0;
        isInDataView = false;
        pages.Add(0, missionPage);
        pages.Add(1, databasePage);
    }

    private void Update() {
        if (isInDataView) return;
        if (currentPageIndex == 0) backButton.SetActive(false);
        else backButton.SetActive(true);

        if (currentPageIndex == pages.Count - 1) nextButton.SetActive(false);
        else nextButton.SetActive(true);
    }

    public void NextPage() {
        if (currentPageIndex == pages.Count - 1) return;
        LoadPage(currentPageIndex+1);
        currentPageIndex++;
    }

    public void LastPage() {
        if (currentPageIndex == 0) return;
        LoadPage(currentPageIndex-1);
        currentPageIndex--;
    }

    private void LoadPage(int pageIndex) {
        pages[currentPageIndex].SetActive(false);
        pages[pageIndex].SetActive(true);
    }

    public void LoadDataPage(string[] data) {
        pages[currentPageIndex].SetActive(false);
        dataPage.SetActive(true);
        isInDataView = true;
        EnableAllButton(false);
        dataPage.GetComponent<MonsterData>().InitializeData(data);
    }

    public void Return() {
        dataPage.SetActive(false);
        pages[currentPageIndex].SetActive(true);
        isInDataView = false;
        EnableAllButton(true);
    }

    private void EnableAllButton(bool isEnabled) {
        nextButton.SetActive(isEnabled);
        backButton.SetActive(isEnabled);
        menuButton.SetActive(isEnabled);
    }
}

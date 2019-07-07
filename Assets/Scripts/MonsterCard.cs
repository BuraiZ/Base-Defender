using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterCard : MonoBehaviour {
    public Text monsterName;
    public Text dangerLevel;
    public Text mainTarget;
    public Text appearance;
    public InstructionMenu instructionMenu;
    public TextAsset textAsset;

    private string[] data;

    private void Start() {
        data = FileHandler.ReadString(textAsset);

        monsterName.text = data[0];
        dangerLevel.text = data[1];
        mainTarget.text = data[2];
        appearance.text = data[3];
    }

    public string[] GetData() {
        return data;
    }

    public void OnCardSelected() {
        instructionMenu.LoadDataPage(data);
    }
}

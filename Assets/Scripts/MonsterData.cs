using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MonsterData : MonoBehaviour {
    public Sprite[] monsterAvatars;
    public Image avatar;
    public Text description;
    public Slider attackPower;
    public Slider defense;
    public Slider attackSpeed;
    public Slider movementSpeed;

    public void InitializeData(string[] data) {
        avatar.sprite = monsterAvatars[Int32.Parse(data[4])];
        description.text = data[5];
        attackPower.value = Int32.Parse(data[6]);
        defense.value = Int32.Parse(data[7]);
        attackSpeed.value = Int32.Parse(data[8]);
        movementSpeed.value = Int32.Parse(data[9]);
    }
}

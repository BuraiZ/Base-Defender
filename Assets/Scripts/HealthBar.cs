using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    private Health health;

    private Rect hpRect;
    
	private void Start () {
        hpRect = GetComponentInParent<RectTransform>().rect;
    }
	
	private void Update () {
        if (!health) return;

        float healthPercentage = health.healthPoint * 1.0f / health.maxHealthPoint;
        GetComponent<RectTransform>().offsetMax =
            new Vector2(-(hpRect.width - healthPercentage * hpRect.width), GetComponent<RectTransform>().offsetMax.y);
    }

    public void SyncObjectToHealthBar(Health objHealth) {
        this.health = objHealth;
    }
}

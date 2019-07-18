using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP_Goal : MonoBehaviour
{
    private LevelEditorControls manager;

    private void OnTriggerEnter(Collider _col) {
        if (_col.gameObject.CompareTag("Ball")){
            manager.FinishLevel();
        }
    }

    private void Awake() {
        manager = GameObject.Find("Managers").GetComponent<LevelEditorControls>();
    }
}

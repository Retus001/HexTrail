using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEditorPieceSpawner : MonoBehaviour
{
    public GameObject piecePfb;
    public int maxAmmount;
    private int currentAmmount;

    public TextMeshProUGUI ammount;

    public int GetAmmount(){
        return currentAmmount;
    }

    public void ReduceAmmount(){
        currentAmmount--;
        SetUIData(currentAmmount);
    }

    public void AddAmmount(){
        currentAmmount++;
        SetUIData(currentAmmount);
    }

    public void ResetAmmount(){
        currentAmmount = maxAmmount;
        SetUIData(currentAmmount);
    }

    public void SetUIData(int _ammountLeft){
        ammount.text = _ammountLeft.ToString();
    }

    public void Awake(){
        currentAmmount = maxAmmount;
        SetUIData(currentAmmount);
    }
}

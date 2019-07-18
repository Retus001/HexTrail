using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP_Main : MonoBehaviour
{
    public enum STATE{
        DYNAMIC = 0,
        STATIC = 1
    };

    STATE currentState = STATE.DYNAMIC;

    public GameObject[] models;
    public ParticleSystem ptr;

    public Material[] m_dynamic;
    public Material[] m_static;

    public void ToggleState(){
        switch(currentState){
            case STATE.DYNAMIC:
                for (int i = 0; i < models.Length; i++){
                    models[i].GetComponent<MeshRenderer>().material = m_static[i];
                    models[i].tag = "PieceStatic";
                }
                currentState = STATE.STATIC;
            break;

            case STATE.STATIC:
                for (int i = 0; i < models.Length; i++){
                    models[i].GetComponent<MeshRenderer>().material = m_dynamic[i];
                    models[i].tag = "PieceDynamic";
                }
                currentState = STATE.DYNAMIC;
            break;

            default:
                Debug.Log("Invalid State");
            break;
        }
    }

    public void ToggleColliders(bool _toggle){
        for (int i = 0; i < models.Length; i++){
            models[i].GetComponent<MeshCollider>().enabled = _toggle;
        }
    }

    public void ToggleParticles(bool _toggle){
        if (_toggle){
            ptr.Play();
        } else {
            ptr.Stop();
        }
    }
}

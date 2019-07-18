using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP_Jumpad : MonoBehaviour
{
    public float jumpForceTM;
    
    private void OnTriggerEnter(Collider _col){
        if (_col.gameObject.CompareTag("Ball")){
            _col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForceTM, ForceMode.Impulse);
        }
    }
}

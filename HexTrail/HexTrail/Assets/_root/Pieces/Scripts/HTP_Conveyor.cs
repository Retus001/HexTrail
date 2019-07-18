using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP_Conveyor : MonoBehaviour
{
    public float conveyorSpeed;

    private void OnTriggerStay(Collider _col) {
        if (_col.CompareTag("Ball")) {
            _col.gameObject.GetComponent<Rigidbody>().velocity = transform.right * conveyorSpeed * Time.deltaTime;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

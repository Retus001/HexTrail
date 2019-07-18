using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP_Y : MonoBehaviour
{
    public GameObject door;
    public float rotSpeed;
    
    public enum DOORSTATE {
        RIGHT,
        LEFT
    }

    private DOORSTATE currentDoorstate = DOORSTATE.RIGHT;

    public void ToggleDoorstate(){
        switch(currentDoorstate){
            case DOORSTATE.RIGHT:
                SpinDoor(Quaternion.Euler(0f, -60f, 0f));
                currentDoorstate = DOORSTATE.LEFT;
            break;

            case DOORSTATE.LEFT:
                SpinDoor(Quaternion.Euler(0f, 0f, 0f));
                currentDoorstate = DOORSTATE.RIGHT;
            break;

            default:
                Debug.Log("Unknown case: " + currentDoorstate);
            break;
        }
    }

    public IEnumerator SpinDoor(Quaternion _rot){
        float comparison = Quaternion.Angle(transform.rotation, _rot);
        if (comparison > 0.01f){
            transform.rotation = Quaternion.Lerp(transform.rotation, _rot, rotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } else {
            transform.rotation = _rot;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorControls : MonoBehaviour
{
    public GameObject ball;

    public Button playBtn;
    public Button stopBtn;

    public SimpleMovement camControls;
    public CameraFollow camPivot;

    private Rigidbody ballRig;
    private Vector3 prevVel;
    private Vector3 initialBallPosition;
    private bool isRunning = false;

    public bool IsRunning(){
        return isRunning;
    }

    public void StartLevel(){
        isRunning = true;
        ballRig.constraints = RigidbodyConstraints.None;
        ballRig.velocity = prevVel;

        playBtn.interactable = false;
        stopBtn.interactable = true;

        camPivot.rotateWithTarget = false;
        camPivot.target = ball;
        camControls.canMove = false;
    }

    public void StopLevel(){
        isRunning = true;
        prevVel = ballRig.velocity;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camPivot.rotateWithTarget = true;
        camPivot.target = camControls.gameObject;
        camControls.canMove = true;
    }

    public void ResetLevel(){
        isRunning = false;
        ball.transform.position = initialBallPosition;
        ball.transform.rotation = Quaternion.identity;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camPivot.rotateWithTarget = true;
        camPivot.target = camControls.gameObject;
        camControls.canMove = true;
    }

    void Start(){
        ballRig = ball.GetComponent<Rigidbody>();
        initialBallPosition = ball.transform.position;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camPivot.rotateWithTarget = true;
        camPivot.target = camControls.gameObject;
        camControls.canMove = true;
    }
}

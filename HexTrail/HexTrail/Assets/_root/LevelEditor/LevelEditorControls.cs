using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorControls : MonoBehaviour
{
    public GameObject ball;

    public Button playBtn;
    public Button stopBtn;

    public GameObject winPanel;

    public GameObject camController;
    private SimpleMovement camControls;
    private CameraFollow camControlsFollow;

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

        camControlsFollow.FollowAll(true);
        camControls.canMove = false;
    }

    public void StopLevel(){
        isRunning = true;
        prevVel = ballRig.velocity;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camControlsFollow.FollowAll(false);
        camControls.canMove = true;
    }

    public void ResetLevel(){
        isRunning = false;
        ball.transform.position = initialBallPosition;
        ball.transform.rotation = Quaternion.identity;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camControlsFollow.FollowAll(false);
        camControls.canMove = true;

        winPanel.SetActive(false);
    }

    public void FinishLevel(){
        ballRig.constraints = RigidbodyConstraints.FreezeAll;
        winPanel.SetActive(true);
    }

    void Start(){
        ballRig = ball.GetComponent<Rigidbody>();
        initialBallPosition = ball.transform.position;
        ballRig.constraints = RigidbodyConstraints.FreezeAll;

        playBtn.interactable = true;
        stopBtn.interactable = false;

        camControls = camController.GetComponent<SimpleMovement>();
        camControlsFollow = camController.GetComponent<CameraFollow>();

        camControlsFollow.FollowAll(false);
        camControls.canMove = true;

        winPanel.SetActive(false);
    }
}

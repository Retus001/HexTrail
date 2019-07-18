using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public GameObject piecesHolder;
    public List<GameObject> pieceList = new List<GameObject>();
    public GameObject grid;
    private GameObject holdedPiece;
    private bool draggingPiece = false;
    public Camera cam;
    public LayerMask pickablesLayermask;
    public LayerMask draggingLayermask;
    private LevelEditorControls LEControls;

    private bool canPlace = true;

    public void SpawnPiece(GameObject _pieceSpawner) {
        if (!LEControls.IsRunning()){
            if (_pieceSpawner.GetComponent<LevelEditorPieceSpawner>().GetAmmount() > 0){
                GameObject pieceToSpawn = _pieceSpawner.GetComponent<LevelEditorPieceSpawner>().piecePfb;
                GameObject piece = Instantiate(pieceToSpawn, grid.transform);
                if (piece.GetComponent<Collider>())
                    piece.GetComponent<Collider>().enabled = false;
                foreach (Transform child in piece.transform){
                    if (child.gameObject.GetComponent<Collider>())
                        child.gameObject.GetComponent<Collider>().enabled = false;
                    if (child.gameObject.GetComponent<ParticleSystem>())
                        child.gameObject.GetComponent<ParticleSystem>().Play();
                }
                holdedPiece = piece;
                draggingPiece = true;
                _pieceSpawner.GetComponent<LevelEditorPieceSpawner>().ReduceAmmount();
            }
        }
    }

    private void SetupPieceSpawners(){
        foreach(Transform child in piecesHolder.transform){
            child.GetComponent<Button>().onClick.AddListener(delegate{SpawnPiece(child.gameObject);});
        }
    }

    private void Update(){
        if (!LEControls.IsRunning()){
            if (draggingPiece){
                if (Input.GetMouseButtonDown(0) && !LEControls.IsRunning()){
                    if (canPlace){
                        draggingPiece = false;
                        if (holdedPiece.GetComponent<Collider>()){
                            holdedPiece.GetComponent<Collider>().enabled = true;}
                        foreach(Transform child in holdedPiece.transform) {
                            if (child.gameObject.GetComponent<Collider>()){
                                child.gameObject.GetComponent<Collider>().enabled = true;}
                            if (child.gameObject.GetComponent<ParticleSystem>()){
                                child.gameObject.GetComponent<ParticleSystem>().Stop();}
                        }
                        holdedPiece = null;
                    }
                } else {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggingLayermask)) {
                        holdedPiece.transform.position = hit.point;
                        holdedPiece.transform.position = grid.GetComponent<Grid>().GetCellCenterWorld(grid.GetComponent<Grid>().WorldToCell(holdedPiece.transform.position));
                        
                        if (hit.collider.gameObject.CompareTag("Grid")){
                            if (!canPlace) {
                                canPlace = true;
                            }
                        } else {
                            if (canPlace) {
                                canPlace = false;
                            }
                        }

                        if (Input.GetKeyDown(KeyCode.E)){
                            holdedPiece.transform.Rotate(Vector3.up * 60, Space.World);
                        } 
                        if (Input.GetKeyDown(KeyCode.Q)){
                            holdedPiece.transform.Rotate(Vector3.down * 60, Space.World);
                        }
                    }
                }
            } else {
                if (Input.GetMouseButtonDown(0) && !LEControls.IsRunning()){
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, pickablesLayermask)) {
                        Debug.Log("Hit: " + hit.collider.gameObject.name);
                        if (hit.collider.gameObject.CompareTag("PieceDynamic")) {
                            holdedPiece = hit.collider.gameObject;
                            holdedPiece.GetComponent<Collider>().enabled = false;
                            draggingPiece = true;
                        } else {
                            if (hit.collider.gameObject.transform.parent) {
                                if (hit.collider.gameObject.transform.parent.gameObject.CompareTag("PieceDynamic")) {
                                    holdedPiece = hit.collider.gameObject.transform.parent.gameObject;
                                    if (holdedPiece.GetComponent<Collider>()){
                                        holdedPiece.GetComponent<Collider>().enabled = false;}
                                    draggingPiece = true;
                                }
                            }
                        }
                        if (holdedPiece){
                            foreach (Transform child in holdedPiece.transform){
                                if (child.gameObject.GetComponent<Collider>()){
                                    child.gameObject.GetComponent<Collider>().enabled = false;}
                                if (child.gameObject.GetComponent<ParticleSystem>()){
                                    child.gameObject.GetComponent<ParticleSystem>().Play();}
                            }
                        } 
                    }
                }
            }
        }
    }

    private void Awake() {
        LEControls = GetComponent<LevelEditorControls>();
    }

    private void Start(){
        cam = Camera.main;
        SetupPieceSpawners();
    }
}

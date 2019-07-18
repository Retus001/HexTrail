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

    public bool editorMode = false;

    private bool canPlace = true;

    public void SpawnPiece(GameObject _pieceSpawner) {
        if (!LEControls.IsRunning()){
            if (_pieceSpawner.GetComponent<LevelEditorPieceSpawner>().GetAmmount() > 0){
                GameObject pieceToSpawn = _pieceSpawner.GetComponent<LevelEditorPieceSpawner>().piecePfb;
                GameObject piece = Instantiate(pieceToSpawn, grid.transform);
                holdedPiece = piece;
                holdedPiece.GetComponent<HTP_Main>().ToggleColliders(false);
                holdedPiece.GetComponent<HTP_Main>().ToggleParticles(true);
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
                        Debug.Log("Place!");
                        draggingPiece = false;
                        holdedPiece.GetComponent<HTP_Main>().ToggleColliders(true);
                        holdedPiece.GetComponent<HTP_Main>().ToggleParticles(false);
                        holdedPiece = null;
                    }
                } else {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggingLayermask)) {
                        Debug.Log("hit: " + hit.collider.gameObject.name);
                        
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
                        if (Input.GetKeyDown(KeyCode.F)){
                            holdedPiece.GetComponent<HTP_Main>().ToggleState();
                        }
                    }
                }
            } else {
                if (Input.GetMouseButtonDown(0) && !LEControls.IsRunning()){
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, pickablesLayermask)) {
                        if (hit.collider.gameObject.CompareTag("PieceDynamic") || editorMode) {
                            holdedPiece = hit.collider.gameObject;
                            draggingPiece = true;
                            if (hit.collider.gameObject.transform.parent != null) {
                                if (hit.collider.gameObject.transform.parent.gameObject.CompareTag("PieceDynamic") || editorMode) {
                                    Debug.Log("parent found:");
                                    holdedPiece = hit.collider.gameObject.transform.parent.gameObject;
                                }
                            }
                        } else {
                            if (hit.collider.gameObject.transform.parent) {
                                if (hit.collider.gameObject.transform.parent.gameObject.CompareTag("PieceDynamic") || editorMode) {
                                    holdedPiece = hit.collider.gameObject.transform.parent.gameObject;
                                    draggingPiece = true;
                                }
                            }
                        }
                        if (holdedPiece){
                            holdedPiece.GetComponent<HTP_Main>().ToggleColliders(false);
                            holdedPiece.GetComponent<HTP_Main>().ToggleParticles(true);
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

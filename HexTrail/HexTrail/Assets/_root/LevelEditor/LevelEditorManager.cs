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

    private bool canPlace = true;

    public void SpawnPiece(GameObject _pieceSpawner) {
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

    private void SetupPieceSpawners(){
        foreach(Transform child in piecesHolder.transform){
            child.GetComponent<Button>().onClick.AddListener(delegate{SpawnPiece(child.gameObject);});
        }
    }

    private void Update(){
        if (draggingPiece){
            if (Input.GetMouseButtonDown(0)){
                if (canPlace){
                    draggingPiece = false;
                    if (holdedPiece.GetComponent<Collider>())
                        holdedPiece.GetComponent<Collider>().enabled = true;
                    if (holdedPiece.GetComponent<MeshRenderer>())
                        holdedPiece.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.black);
                    foreach(Transform child in holdedPiece.transform) {
                        if (child.gameObject.GetComponent<Collider>())
                            child.gameObject.GetComponent<Collider>().enabled = true;
                        if (child.gameObject.GetComponent<ParticleSystem>())
                            child.gameObject.GetComponent<ParticleSystem>().Stop();
                        if (child.gameObject.GetComponent<MeshRenderer>())
                            child.gameObject.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.black);
                    }
                }
            } else {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
 
                if (Physics.Raycast(ray, out hit, pickablesLayermask)) {
                    holdedPiece.transform.position = hit.point;
                    holdedPiece.transform.position = grid.GetComponent<Grid>().GetCellCenterWorld(grid.GetComponent<Grid>().WorldToCell(holdedPiece.transform.position));
                    
                    if (hit.collider.gameObject.CompareTag("Grid")){
                        if (!canPlace) {
                            canPlace = true;
                            if (holdedPiece.GetComponent<MeshRenderer>())
                                holdedPiece.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.cyan);
                            else {
                                foreach(Transform child in holdedPiece.transform) {
                                    child.gameObject.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.cyan);
                                }
                            }
                        }
                    } else {
                        if (canPlace) {
                            canPlace = false;
                            if (holdedPiece.GetComponent<MeshRenderer>())
                                holdedPiece.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.red);
                            else {
                                foreach(Transform child in holdedPiece.transform) {
                                    child.gameObject.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", Color.red);
                                }
                            }
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
            if (Input.GetMouseButtonDown(0)){
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, pickablesLayermask)) {
                    if (hit.collider.gameObject.CompareTag("PieceDynamic")) {
                        holdedPiece = hit.collider.gameObject;
                        holdedPiece.GetComponent<Collider>().enabled = false;
                        draggingPiece = true;
                    } else {
                        if (hit.collider.gameObject.transform.parent) {
                            if (hit.collider.gameObject.transform.parent.gameObject.CompareTag("PieceDynamic")) {
                                holdedPiece = hit.collider.gameObject.transform.parent.gameObject;
                                if (holdedPiece.GetComponent<Collider>())
                                    holdedPiece.GetComponent<Collider>().enabled = false;
                                draggingPiece = true;
                            }
                        }
                    }
                    foreach (Transform child in holdedPiece.transform){
                        if (child.gameObject.GetComponent<Collider>())
                            child.gameObject.GetComponent<Collider>().enabled = false;
                        if (child.gameObject.GetComponent<ParticleSystem>())
                            child.gameObject.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }
    }

    private void Start(){
        cam = Camera.main;
        SetupPieceSpawners();
    }
}

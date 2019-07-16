using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneUIManager : MonoBehaviour
{
    public GameObject pausePanel;
    
    public void TogglePause(bool _toggle){
        pausePanel.SetActive(_toggle);
    }

    public void ExitToMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

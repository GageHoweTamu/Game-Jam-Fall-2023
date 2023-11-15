using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseButton : MonoBehaviour
{
    public Canvas pauseMenu;
    public PlayerController3 parasiteScript;
    // Start is called before the first frame update
    void Start()
    {
        parasiteScript = GameObject.Find("PlayerParasite").GetComponent<PlayerController3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressed()
    {
        Debug.Log("Button Pressed");

            Time.timeScale = 0;
            pauseMenu.enabled = true;
            

    }

    public void OnResumeButton()
    {
        Time.timeScale = 1;
        pauseMenu.enabled = false;
    }

    public void onStartButton()
    {
        pauseMenu.enabled = false;
        SceneManager.LoadScene("Level1_DUPE");
    }

    public void onQuitButton()
    {
        Application.Quit();
    }

    public void onHomeButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScreen");
    }

    public void onResetButton()
    {
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        parasiteScript.Die();
    }
}

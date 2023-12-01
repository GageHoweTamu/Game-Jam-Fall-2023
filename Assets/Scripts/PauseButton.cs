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
    [SerializeField] private AudioClip clickSound;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        parasiteScript = GameObject.Find("PlayerParasite").GetComponent<PlayerController3>();
        audioSource = GetComponent<AudioSource>();
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
        //audioSource.PlayOneShot(clickSound);
        parasiteScript.paused = true;


    }

    public void OnResumeButton()
    {
        audioSource.PlayOneShot(clickSound);
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        parasiteScript.paused = false;
    }

    public void onStartButton()
    {
        audioSource.PlayOneShot(clickSound);
        pauseMenu.enabled = false;
        SceneManager.LoadScene("Level1_FINAL");
    }

    public void onQuitButton()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }

    public void onHomeButton()
    {
        audioSource.PlayOneShot(clickSound);
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScreen");
    }

    public void onResetButton()
    {
        audioSource.PlayOneShot(clickSound);
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        parasiteScript.Die();
        parasiteScript.paused = false;
    }
}

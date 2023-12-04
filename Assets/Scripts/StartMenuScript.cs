using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void onStartButton()
    {
        SceneManager.LoadScene("Level1_FINAL");
    }

    public void onQuitButton()
    {
        Application.Quit();
    }
}

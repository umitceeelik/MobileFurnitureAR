using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetFramerate : MonoBehaviour
{   
    public int framerate;

    void Awake()
    {
        Application.targetFrameRate = framerate;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Sample", LoadSceneMode.Single);
    }
}

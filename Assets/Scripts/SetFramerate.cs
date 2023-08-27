using AtlasSpace.World;
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
        GetParameterWithUrl.Instance.IsBackFromARScene = true;
        SceneManager.LoadScene("Main");
    }
}

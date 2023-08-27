using AtlasSpace.World;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProductController : MonoBehaviour
{
    public Image productImage;
    public string productName;
    public string companyName;

    private GetParameterWithUrl getParameterWithUrl;

    private void Awake()
    {
        getParameterWithUrl = GetParameterWithUrl.Instance;
    }
    public void OpenARScene()
    {
        SetProductParameter();
        SceneManager.LoadScene("ARScene");
    }

    private void SetProductParameter()
    {
        string productUrl = $"{getParameterWithUrl.baseUrl}{companyName}/{productName}/{productName}.glb";
        getParameterWithUrl.FinalProductUrl = productUrl;
        getParameterWithUrl.IsBackFromARSceneCompanyName = companyName;
    }
}

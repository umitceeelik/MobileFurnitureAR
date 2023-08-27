using AtlasSpace.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class CompanyGetter : MonoBehaviour
{
    public GameObject companyPrefab;
    public Transform companyHolder;
    public Transform productHolder;
    public List<CompanyController> companyList;
    public SerializedDictionary<string, CompanyController> companyDictonary;
    public GameObject companyPanel;
    public GameObject productPanel;
    public GameObject mainMenuPanel;
    public GameObject blackScreen;

    public static CompanyGetter Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartCoroutine(GetCompanies());
    }

    private void Start()
    {
        if (!GetParameterWithUrl.Instance.IsBackFromARScene)
            blackScreen.SetActive(false);

        //if (GetParameterWithUrl.Instance.IsOpenedWithUrl)
        //{

        //}
        //else
        //{

        //    companyPanel.SetActive(true);
        //    productPanel.SetActive(false);
        //}
    }

    public IEnumerator GetCompanies() // When click the button, gets the data of the object.
    {
        // the url will be changed when the database ok. It will be get api of companies
        //string url = $"https://api-stage.atlas.space/api/Wix/GetWixProductDetailsById?productId=";
        string url = $"http://foresightapi-dev.eu-west-1.elasticbeanstalk.com/api/Users/GetUsers";
        //string url = $"http://foresightapi-dev.eu-west-1.elasticbeanstalk.com/api/Furniture/GetFurnitures";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var text = request.downloadHandler.text;
            //List<AppUser> users = JsonUtility.FromJson<List<AppUser>>(text);

            AppUser[] users = GeneralApiResponse.ParseJsonArray<AppUser>(text);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                foreach (AppUser user in users)
                {
                    GameObject createdProduct = Instantiate(companyPrefab, companyHolder);
                    CompanyController companyController = createdProduct.GetComponent<CompanyController>();
                    companyList.Add(companyController);
                    companyController.companyId = user.UserId;
                    companyController.companyName.text = user.CompanyName;
                    companyController.productHolder = productHolder;
                    companyDictonary.Add(user.CompanyName, companyController);

                    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(user.CompanyImageUrl))
                    {
                        yield return webRequest.SendWebRequest();

                        if (webRequest.result == UnityWebRequest.Result.Success)
                        {
                            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                            companyController.companyImage.sprite = sprite;

                            RectTransform companyImageRect = companyController.companyImage.gameObject.GetComponent<RectTransform>();
                            var maxXValue = companyImageRect.sizeDelta.x;
                            companyController.companyImage.SetNativeSize();
                            companyImageRect.sizeDelta = StretchImage(companyImageRect.sizeDelta, maxXValue);
                        }
                        else
                        {
                            Debug.Log("URL'ye eri�ilemedi. Hata: " + webRequest.error);
                        }
                    }
                }
            }

            if (GetParameterWithUrl.Instance.IsBackFromARScene)
            {
                GetParameterWithUrl.Instance.BackFromARScene(GetParameterWithUrl.Instance.IsBackFromARSceneCompanyName);
            }
        }
    }

    public Vector2 StretchImage(Vector2 sizedelta, float maxXValue)
    {
        float number = (maxXValue - 10) / sizedelta.x;
        Vector2 resultVector2 = new Vector2(sizedelta.x * number, sizedelta.y * number);
        return resultVector2;
    }

    [System.Serializable]
    public class AppUser
    {
        public string UserId;
        public string CompanyName;
        public string CompanyImageUrl;
        public string UserName;
        public string Email;
    }
}

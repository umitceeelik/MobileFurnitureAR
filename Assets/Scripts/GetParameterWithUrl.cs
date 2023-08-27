using AtlasSpace.UI;
using System;
using System.Web;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

namespace AtlasSpace.World
{
    public class GetParameterWithUrl : MonoBehaviour
    {
        // the product link can be "https://furniture.app/companyName/productName/product.glb"
        // the deep link of the app can be "furniture://furniture?companyName=companyName&productName=product&product.glb"


        [SerializeField] public string ARScene = "ARScene";

        //public string bimARGameScene;
        public string baseUrl = "https://d39z5cwudrto0g.cloudfront.net/";

        public string FinalProductUrl;
        public string FinalProductImageUrl;

        public bool IsBackFromARScene { get; set; }
        public string IsBackFromARSceneCompanyName { get; set; }

        public static GetParameterWithUrl Instance { get; private set; }
        private void Awake()
        {
            //var uri = new Uri("furniture://furniture?companyName=BMS&productName=Cartellinne");     //atlasspace://atlasspace?bimargame=true

            //string companyName = HttpUtility.ParseQueryString(uri.Query).Get("companyName");
            //string productName = HttpUtility.ParseQueryString(uri.Query).Get("productName");
            //Debug.Log("companyName = " + companyName);
            //Debug.Log("productName = " + productName);

            //SetParamToUrl("furniture://furniture?companyName=BMS&productName=Cartellinne");
            
            //Uri myUri = new Uri("http://www.coderseditor.com?param1=tutorial&param2=onlineide");
            //string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("param1");


            if (Instance == null)
            {
                Instance = this;
                Application.deepLinkActivated += OnDeepLinkActivated;
                if (!string.IsNullOrEmpty(Application.absoluteURL))
                {
                    // blackScreen.SetActive(true);
                    // Cold start and Application.absoluteURL not null so process Deep Link.
                    OnDeepLinkActivated(Application.absoluteURL);
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    OpenWithApplication();
                }
                DontDestroyOnLoad(gameObject);

                // Initialize DeepLink Manager global variable.
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // When starting app with url, what will we do.
        private void OnDeepLinkActivated(string url) // url = "furniture://furniture?companyName=BMS&productName=Cartellinne"
        {
            SetParamToUrl(url);
            LoadARScene();
            //var (param, value) = GetParam(url);
            //switch (param)
            //{
            //    case "bimargame":
            //        //isBimScene = true;
            //        break;
            //        //case y:
            //        //    // code block
            //        //    break;
            //        //default:
            //        //    // code block
            //        //    break;
            //}
        }

        // To get parameter from url which launchs the app.
        private (string, string) GetParam(string link)
        {
            //foresightar://foresightar?companyname=true
            var uri = new Uri(link);     //atlasspace://atlasspace?bimargame=true
            string query = uri.Query;   //?bimargame=true
            string param = query.Split('=')[0].Remove(0, 1); //bimargame
            string value = query.Split('=')[1]; //true

            switch (query)
            {
                case "bimargame":
                    //isBimScene = true;
                    break;
            }

            return (param, value);
        }

        private void SetParamToUrl(string url)
        {
            var uri = new Uri(url);     //foresightar://foresightar?companyname=BMS&productName=masa

            string companyName = HttpUtility.ParseQueryString(uri.Query).Get("companyName");
            string productName = HttpUtility.ParseQueryString(uri.Query).Get("productName");
            Debug.Log("companyName = " + companyName);
            Debug.Log("productName = " + productName);
            FinalProductUrl = $"{baseUrl}{companyName}/{productName}/{productName}.glb";
            FinalProductImageUrl = $"{baseUrl}{companyName}/{productName}/{productName}.png";
            IsBackFromARSceneCompanyName = companyName;
            //BackFromARScene(companyName);
            Debug.Log($"product url = {FinalProductUrl}");
            Debug.Log($"product image url = {FinalProductImageUrl}");
        }

        // To load given scene with addressable.
        //public void LoadBimARGameScene(AssetReference scene)
        //{
        //    scene.LoadSceneAsync(LoadSceneMode.Single).Completed += (asyncHandle) =>
        //    {
        //        Debug.Log("AR Scene Loaded Successfully");
        //    };
        //}

        // To load AR scene
        public void LoadARScene()
        {
            SceneManager.LoadScene("ARScene");
        }

        public void OpenWithApplication()
        {
            CompanyGetter.Instance.companyPanel.SetActive(true);
            CompanyGetter.Instance.productPanel.SetActive(false);
        }

        public void BackFromARScene(string companyName)
        {
            StartCoroutine(CompanyGetter.Instance.companyDictonary[companyName].GetCompanyProducts());
        }
    }
}

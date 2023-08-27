using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CompanyController : MonoBehaviour
{
    public string companyId;
    public Image companyImage;
    public TextMeshProUGUI companyName;
    //public TextMeshProUGUI companyInfo;
    public GameObject productPrefab;
    public Transform productHolder;
    public List<ProductController> productList;
  
    public void GetProducts()
    {
        for (int i = productHolder.childCount; 0 < i; i--)
        {
            Destroy(productHolder.GetChild(i-1).gameObject);
        }

        StartCoroutine(GetCompanyProducts());
    }

    public IEnumerator GetCompanyProducts() // When click the button, gets the data of the object.
    {
        // the url will be changed when the database ok. It will be get api of company products.
        Guid guid = new Guid(companyId);
        string url = $"http://foresightapi-dev.eu-west-1.elasticbeanstalk.com/api/Furniture/GetFurnituresByUserId?userId={guid}"; 
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var text = request.downloadHandler.text;
            Furniture[] furnitures = GeneralApiResponse.ParseJsonArray<Furniture>(text);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                foreach (Furniture furniture in furnitures)
                {
                    GameObject createdProduct = Instantiate(productPrefab, productHolder);
                    ProductController productController = createdProduct.GetComponent<ProductController>();
                    productList.Add(productController);
                    productController.productName = furniture.Name;
                    productController.companyName = companyName.text;

                    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(furniture.ObjectImageUrl))
                    {
                        yield return webRequest.SendWebRequest();

                        if (webRequest.result == UnityWebRequest.Result.Success)
                        {
                            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                            productController.productImage.sprite = sprite;

                            RectTransform productImageRect = productController.productImage.gameObject.GetComponent<RectTransform>();
                            var maxXValue = productImageRect.sizeDelta.x;
                            productController.productImage.SetNativeSize();
                            productImageRect.sizeDelta = CompanyGetter.Instance.StretchImage(productImageRect.sizeDelta, maxXValue);
                        }
                        else
                        {
                            Debug.Log("URL'ye eri�ilemedi. Hata: " + webRequest.error);
                        }
                    }
                }

                CompanyPanelController.Instance.CompanyInfoFiller(companyImage.sprite, companyName.text, companyName.text);
                //CompanyGetter.Instance.companyPanel.SetActive(false);
                //CompanyGetter.Instance.productPanel.SetActive(true);
            }
        }
    }

    [Serializable]
    public class Furniture
    {
        public string Id;
        public string Name;
        public int Price;
        public string ObjectUrl;
        public string ObjectImageUrl;
        public string QRCodeUrl;
        public Guid CreatedBy;
    }
}

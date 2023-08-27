using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanyPanelController : MonoBehaviour
{
    public Image companyImage;
    public TextMeshProUGUI companyName;
    public TextMeshProUGUI companyInfo;


    public static CompanyPanelController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CompanyInfoFiller(Sprite companyImage, string companyName, string companyInfo)
    {
        this.companyImage.sprite = companyImage;
        RectTransform imageRect = this.companyImage.gameObject.GetComponent<RectTransform>();
        var maxXValue = imageRect.sizeDelta.x;
        this.companyImage.SetNativeSize();
        imageRect.sizeDelta = CompanyGetter.Instance.StretchImage(imageRect.sizeDelta, maxXValue);

        this.companyName.text = companyName;
        this.companyInfo.text = companyInfo;

        CompanyGetter.Instance.companyPanel.SetActive(false);
        CompanyGetter.Instance.mainMenuPanel.SetActive(false);
        CompanyGetter.Instance.productPanel.SetActive(true);
        CompanyGetter.Instance.blackScreen.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] public GameObject scanUI;
    [SerializeField] Button placeObjectButton;
    [SerializeField] TextMeshProUGUI placeObjectButtonText;
    [SerializeField] Color placeObjectButtonTextColor;

    private Color placeObjectButtonTextInitialColor;
    public static UIController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        placeObjectButtonTextInitialColor = placeObjectButtonText.color;
        ObjectPlacementSetter(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectPlacementSetter(bool canPlace)
    {
        if (canPlace)
        {
            scanUI.SetActive(false);
            placeObjectButton.interactable = true;
            placeObjectButtonText.color = placeObjectButtonTextInitialColor;
        }
        else
        {
            scanUI.SetActive(true);
            placeObjectButton.interactable = false;
            placeObjectButtonText.color = placeObjectButtonTextColor;
        }
    }


}

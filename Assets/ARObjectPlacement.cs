using GLTFast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    public class ARObjectPlacement : MonoBehaviour
    {
        public GltfAsset gltfAsset;
        [SerializeField] TextMeshProUGUI text;

        public GameObject arObjectToSpawn;
        //public GameObject arObjectToSpawn2;
        public GameObject placementIndicator;
        //public GameObject canvas;

        private GameObject spawnedObject;
        private Pose PlacementPose;

        [SerializeField]
        private ARRaycastManager aRRaycastManager;
        [SerializeField]
        private ARPlaneManager aRPlaneManager;
        [SerializeField]
        private XROrigin xROrigin;

        private bool placementPoseIsValid = false;
        private bool isSpawned = false;

        bool isWaiting = false;
        bool isLoadedObject = false;
        //public TextMeshProUGUI testText;

        //public TextMeshProUGUI text;
        //public TextMeshProUGUI textTouch;

        public static ARObjectPlacement Instance;

        [Header("Loading Bar")]
        [SerializeField] GameObject loadingBar;
        [SerializeField] TextMeshProUGUI loadingBarText;
        [SerializeField] UnityEngine.UI.Image loadingBarImg;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            //arObjectToSpawn = FindObjectOfType<LoadSceneWithARCamera>().ARObject;
        }

        void Start()
        {
            placementIndicator.SetActive(false);
        }

        // need to update placement indicator, placement pose and spawn 
        void Update()
        {
            if (!aRRaycastManager.enabled || !aRPlaneManager.enabled)
                return;

            if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isSpawned)
            {

                //AddressableObjectImporter.Instance.ImportARObject();
                //addressableObjectImporter.ARObjectToSpawn.SetActive(true);
                //arObjectToSpawn = addressableObjectImporter.ARObjectToSpawn;

                //if (!(gltfAsset.transform.childCount > 0) && !isWaiting)
                //{
                //    LoadObject(); 
                //}     
                //else
                //{
                //text.text = gltfAsset.transform.GetChild(0).name;
                //arObjectToSpawn = gltfAsset.gameObject;
                //StartCoroutine(ARPlaceObject());
                //}
            }

            if (spawnedObject == null)
            {
                UpdatePlacementPose();
                UpdatePlacementIndicator();
            }


        }

        public void SPAWN()
        {
            StartCoroutine(ARPlaceObject());
            UIController.Instance.ObjectPlacementSetter(false);
            //StartCoroutine(LoadGLBWithProgress());
        }

        public async void LoadObject()
        {
            isWaiting = true;
            isWaiting = await gltfAsset.Load(gltfAsset.Url);


            //text.text = gltfAsset.transform.GetChild(0).name;
            arObjectToSpawn = gltfAsset.gameObject;

            isLoadedObject = true;
            //arObjectToSpawn.SetActive(false);
            //ARPlaceObject();
        }

        void UpdatePlacementIndicator()
        {
            if (spawnedObject == null && placementPoseIsValid && !placementIndicator.activeSelf)
            {
                placementIndicator.SetActive(true);
                UIController.Instance.ObjectPlacementSetter(true);
            }
            else if (placementIndicator.activeSelf && !placementPoseIsValid)
            {
                placementIndicator.SetActive(false);
                UIController.Instance.ObjectPlacementSetter(false);
                //testText.text = "false/ " + spawnedObject?.activeSelf.ToString() + " / " + placementPoseIsValid;
            }
        }
        void UpdatePlacementPose()
        {
            //testText.text = "Girdiiiiii2222222";
            //textTouch.text = placementPoseIsValid.ToString();
            //var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;

            if (placementPoseIsValid)
            {
                placementIndicator.transform.position = hits[0].pose.position;
                xROrigin.CameraYOffset = placementIndicator.transform.position.y;
                //text.text = xROrigin.CameraYOffset.ToString();
            }
        }

        IEnumerator ARPlaceObject()
        {
            //LoadObject();

            StartCoroutine(LoadGLBWithProgress());

            yield return new WaitUntil(() => isLoadedObject);
            isSpawned = true;
            //arObjectToSpawn.transform.SetPositionAndRotation(placementIndicator.transform.position, Quaternion.identity);
            spawnedObject = arObjectToSpawn;
            spawnedObject.transform.localPosition = placementIndicator.transform.position;
            //spawnedObject.transform.SetPositionAndRotation(placementIndicator.transform.position, Quaternion.identity);
            spawnedObject.SetActive(true);
            StartCoroutine(LerpObjectScale(Vector3.zero, spawnedObject.transform.localScale, 1f, spawnedObject));
            placementIndicator.SetActive(false);
            aRPlaneManager.enabled = false;
            aRRaycastManager.enabled = false;
            UIController.Instance.scanUI.SetActive(false);
        }

        public void ARPlaceObject2()
        {
            if (isSpawned)
                return;
            isSpawned = true;
            spawnedObject = Instantiate(arObjectToSpawn, placementIndicator.transform.position, Quaternion.Euler(Vector3.zero));
            placementIndicator.SetActive(false);
            isSpawned = true;
        }

        IEnumerator LerpObjectScale(Vector3 a, Vector3 b, float time, GameObject lerpObject)
        {
            float i = 0.0f;
            float rate = (1.0f / time);
            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                lerpObject.transform.localScale = Vector3.Lerp(a, b, i);
                yield return null;
            }
        }

        private IEnumerator LoadGLBWithProgress()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(gltfAsset.Url))
            {
                // Send the request
                var asyncLoad = www.SendWebRequest();
                loadingBar.SetActive(true);
                while (!asyncLoad.isDone)
                {
                    // Update the progress bar value based on the loading progress
                    Debug.Log((int)(Mathf.Clamp01(asyncLoad.progress) * 100));

                    float currentValue = (int)(Mathf.Clamp01(asyncLoad.progress) * 100);

                    if (currentValue < 100)
                        loadingBarText.text = currentValue + "%";
                    else
                        loadingBarText.text = "Done";

                    loadingBarImg.fillAmount = currentValue / 100;

                    yield return null; // Wait for the next frame
                }

                if (asyncLoad.isDone)
                {
                    loadingBarImg.fillAmount = 100;
                    loadingBarText.text = "Done";
                }

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("GLB loading failed.");
                }
                else
                {
                    string filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "downloaded.glb");

                    // Save the downloaded data to a file
                    File.WriteAllBytes(filePath, www.downloadHandler.data);

                    // GLB loading is complete, you might want to parse and instantiate the loaded object here
                    byte[] data = www.downloadHandler.data;



                    //GltfAsset gltfAsset = ScriptableObject.CreateInstance<GltfAsset>();
                    LoadDownloadedObject(data, filePath);
                    //gltfAsset.Importer.LoadGltfBinary(data);
                    // Instantiate(gltfAsset.scene);
                }
            }
        }

        private async void LoadDownloadedObject(byte[] data, string filePath)
        {
            var gltf = new GltfImport();
            bool success = await gltf.LoadGltfBinary(
                    data,
                    // The URI of the original data is important for resolving relative URIs within the glTF
                    new Uri(filePath)
                    );
            if (success)
            {
                Debug.Log("Girdi");
                success = await gltf.InstantiateMainSceneAsync(transform);

                if (success)
                {
                    loadingBar.SetActive(false);
                    arObjectToSpawn = transform.GetChild(1).gameObject;
                    isLoadedObject = true;
                }

                Debug.Log(success);
            }
        }
    }
}


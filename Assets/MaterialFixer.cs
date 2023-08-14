using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialFixer : MonoBehaviour
{

    GameObject loadedObject;

    [SerializeField] TextMeshProUGUI text;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (loadedObject == null && transform.childCount > 1)
        {
            loadedObject = transform.GetChild(1).gameObject;
            //MeshRenderer renderer = loadedObject.GetComponent<MeshRenderer>();
            //Material material = renderer.material;
            //text.text += material.name;
            //text.text += material.color;
            //text.text += material.mainTexture.name;
        }
    }

    public void DoIT()
    {
        MeshRenderer renderer = loadedObject.GetComponent<MeshRenderer>();
        Material material = renderer.material;
        //text.text += material.name;
        //text.text += material.color;
        //text.text += material.mainTexture.name;
        //material.color = Color.white;
        //text.text += material.color;
        //originalTexture = (Texture2D)material.mainTexture;
        //TextureConverter();
        //material.mainTexture = convertedTexture;
    }

    public Texture2D originalTexture; // The 8k texture to convert
    public Texture2D convertedTexture; // The resulting 2k texture

    public void TextureConverter()
    {
        // Check if the original texture is not null
        if (originalTexture != null)
        {
            //int targetWidth = 2048; // 2k width
            //int targetHeight = 1024; // 2k height

            //// Create a new 2k texture
            //convertedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            //// Resize and copy data from the original texture to the new one
            //originalTexture.isReadable = true;
            //for (int y = 0; y < targetHeight; y++)
            //{
            //    for (int x = 0; x < targetWidth; x++)
            //    {
            //        Color originalPixel = originalTexture.GetPixelBilinear((float)x / targetWidth, (float)y / targetHeight);
            //        convertedTexture.SetPixel(x, y, originalPixel);
            //    }
            //}

            //// Apply the changes to the new texture
            //convertedTexture.Apply();


            //int targetWidth = 2048; // 2k width
            //int targetHeight = 1024; // 2k height

            //// Make the original texture readable
            //originalTexture.filterMode = FilterMode.Point; // Ensure pixel-perfect read
            //RenderTexture renderTex = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            //Graphics.Blit(originalTexture, renderTex);
            //RenderTexture previous = RenderTexture.active;
            //RenderTexture.active = renderTex;
            //convertedTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
            //convertedTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            //convertedTexture.Apply();
            //RenderTexture.active = previous;
            //RenderTexture.ReleaseTemporary(renderTex);

            //// Resize the new texture to 2k
            //TextureScale.Bilinear(convertedTexture, targetWidth, targetHeight);

            int targetWidth = 2048; // 2k width
            int targetHeight = 1024; // 2k height

            //// Create a new RenderTexture with 2k resolution
            //RenderTexture tempRT = new RenderTexture(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            //// Set the original 8k texture as the source of the RenderTexture
            //Graphics.Blit(originalTexture, tempRT);

            //// Create a new 2k texture and copy the content from the RenderTexture
            //convertedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
            //RenderTexture.active = tempRT;
            //convertedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            //convertedTexture.Apply();

            //// Release the temporary RenderTexture
            //RenderTexture.active = null;
            //tempRT.Release();
            //Destroy(tempRT);
            /*
            RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(originalTexture, rt);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
            resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);
            1

            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(originalTexture, rt);

            Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = previous;

            RenderTexture.ReleaseTemporary(rt);
            2
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            rt.filterMode = FilterMode.Point; // Set the filter mode to Point

            Graphics.Blit(originalTexture, rt);

            Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = previous;

            RenderTexture.ReleaseTemporary(rt);
            convertedTexture = resizedTexture;
            */

            Color[] sourcePixels = originalTexture.GetPixels();
            Color[] resizedPixels = new Color[targetWidth * targetHeight];

            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    float u = (float)x / targetWidth;
                    float v = (float)y / targetHeight;

                    // Sample the color with bilinear filtering (no point filtering)
                    Color color = SampleBilinear(sourcePixels, originalTexture.width, originalTexture.height, u, v);

                    // Apply gamma correction when setting the pixel color
                    resizedPixels[y * targetWidth + x] = color.gamma;
                }
            }

            Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
            resizedTexture.SetPixels(resizedPixels);
            resizedTexture.Apply();
            //RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            //Graphics.Blit(originalTexture, rt);

            //RenderTexture previous = RenderTexture.active;
            //RenderTexture.active = rt;

            //Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            //// Apply gamma correction when reading pixels
            //Color[] pixels = resizedTexture.GetPixels();
            //for (int i = 0; i < pixels.Length; i++)
            //{
            //    pixels[i].r = Mathf.LinearToGammaSpace(pixels[i].r);
            //    pixels[i].g = Mathf.LinearToGammaSpace(pixels[i].g);
            //    pixels[i].b = Mathf.LinearToGammaSpace(pixels[i].b);

            //}

            //resizedTexture.SetPixels(pixels);
            //resizedTexture.Apply();

            //RenderTexture.active = previous;
            //RenderTexture.ReleaseTemporary(rt);

            //RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            //Graphics.Blit(originalTexture, rt);

            //RenderTexture previous = RenderTexture.active;
            //RenderTexture.active = rt;

            //Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            //// Apply gamma correction when setting pixels
            //Color[] pixels = new Color[targetWidth * targetHeight];
            //for (int y = 0; y < targetHeight; y++)
            //{
            //    for (int x = 0; x < targetWidth; x++)
            //    {
            //        Color pixelColor = originalTexture.GetPixelBilinear((float)x / targetWidth, (float)y / targetHeight);
            //        pixelColor.r = Mathf.GammaToLinearSpace(pixelColor.r);
            //        pixelColor.g = Mathf.GammaToLinearSpace(pixelColor.g);
            //        pixelColor.b = Mathf.GammaToLinearSpace(pixelColor.b);
            //        pixels[y * targetWidth + x] = pixelColor;
            //    }
            //}

            //resizedTexture.SetPixels(pixels);
            //resizedTexture.Apply();

            //RenderTexture.active = previous;
            //RenderTexture.ReleaseTemporary(rt);

            //convertedTexture = resizedTexture;
        }
    }

    private Color SampleBilinear(Color[] pixels, int width, int height, float u, float v)
    {
        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);

        float x = u * (width - 1);
        float y = v * (height - 1);
        int x0 = Mathf.FloorToInt(x);
        int y0 = Mathf.FloorToInt(y);
        int x1 = Mathf.Min(x0 + 1, width - 1);
        int y1 = Mathf.Min(y0 + 1, height - 1);

        float uRatio = x - x0;
        float vRatio = y - y0;

        Color bottomLeft = pixels[y0 * width + x0];
        Color bottomRight = pixels[y0 * width + x1];
        Color topLeft = pixels[y1 * width + x0];
        Color topRight = pixels[y1 * width + x1];

        // Bilinear interpolation
        Color result = Color.Lerp(Color.Lerp(bottomLeft, bottomRight, uRatio), Color.Lerp(topLeft, topRight, uRatio), vRatio);
        return result;
    }
}

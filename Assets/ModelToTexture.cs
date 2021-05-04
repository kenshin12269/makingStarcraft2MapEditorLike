using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ModelToTexture : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera renderCam;
    public GameObject[] captureTargetPrefabs;
    private string fileRootPath
    {
        get
        {
            return Application.dataPath + "/ModelCapture";
        }
    }
    void Start()
    {

    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Render"))
        {
            StartCoroutine(Capture());
        }
    }
    IEnumerator Capture()
    {
        GameObject bc = GameObject.Instantiate<GameObject>(renderCam.gameObject);
        var blackCam = bc.GetComponent<Camera>();
        blackCam.backgroundColor = Color.black;
        blackCam.clearFlags = CameraClearFlags.SolidColor;
        blackCam.tag = "MainCamera";

        GameObject wc = GameObject.Instantiate<GameObject>(renderCam.gameObject);
        var whiteCam = wc.GetComponent<Camera>();
        whiteCam.backgroundColor = Color.white;
        whiteCam.clearFlags = CameraClearFlags.SolidColor;

        // yield return new WaitForSeconds(1.0f);
        yield return new WaitForEndOfFrame();
        var blackCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        var whiteCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

        blackCam.targetTexture = blackCamRenderTexture;
        whiteCam.targetTexture = whiteCamRenderTexture;

        for (int i = 0; i < captureTargetPrefabs.Length; i++)
        {
            Debug.LogFormat("Capture={0}", captureTargetPrefabs[i].name);
            yield return StartCoroutine(CaptureSingle(captureTargetPrefabs[i], blackCam, whiteCam, blackCamRenderTexture, whiteCamRenderTexture));
        }
        GameObject.Destroy(bc);
        GameObject.Destroy(wc);
        blackCamRenderTexture.Release();
        whiteCamRenderTexture.Release();
    }
    IEnumerator CaptureSingle(GameObject prefabObj,
    Camera blackCam, Camera whiteCam,
    RenderTexture blackCamRenderTexture,
    RenderTexture whiteCamRenderTexture)
    {
        var created = GameObject.Instantiate<GameObject>(prefabObj);
        yield return new WaitForEndOfFrame();
        blackCam.Render();
        RenderTexture.active = blackCamRenderTexture;
        var texb = GetTex2D(true);

        //Now do it for Alpha Camera

        whiteCam.Render();
        RenderTexture.active = whiteCamRenderTexture;
        var texw = GetTex2D(true);


        int width = Screen.width;
        int height = Screen.height;

        var outputtex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // Create Alpha from the difference between black and white camera renders
        for (int y = 0; y < outputtex.height; ++y)
        { // each row
            for (int x = 0; x < outputtex.width; ++x)
            { // each column
                float alpha;
                alpha = texw.GetPixel(x, y).r - texb.GetPixel(x, y).r;
                alpha = 1.0f - alpha;
                Color color;

                if (alpha < 0.01f)
                {
                    color = Color.clear;
                }
                else
                {
                    color = texb.GetPixel(x, y) / alpha;
                }
                color.a = alpha;
                outputtex.SetPixel(x, y, color);
            }
        }

        // Encode the resulting output texture to a byte array then write to the file
        byte[] pngShot = outputtex.EncodeToPNG();
        if (System.IO.Directory.Exists(fileRootPath) == false)
            System.IO.Directory.CreateDirectory(fileRootPath);
        var filePath = string.Format("{0}/{1}.png", fileRootPath, prefabObj.name);
        File.WriteAllBytes(filePath, pngShot);
        GameObject.Destroy(created);
    }

    private Texture2D GetTex2D(bool renderAll)
    {
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        if (!renderAll)
        {
            width = width / 2;
        }

        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        return tex;
    }
}

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using System.IO;
using System.Collections;

public class ScreenshotCam : MonoBehaviour
{
    private static ScreenshotCam _instance;
    public static ScreenshotCam Instance {get {return _instance;}}
    public RenderTexture rTex;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    public void Screenshot(string filepath){
        StartCoroutine(TakeScreenshot(filepath));
    }

    IEnumerator TakeScreenshot(string filepath){
        yield return null;

        // Texture2D tex = new Texture2D(3840, 2160, GraphicsFormatUtility.GetTextureFormat(rTex.graphicsFormat), false, true);
        Texture2D tex = new Texture2D(3840, 2160, TextureFormat.RGB565, false, true);
        
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        
        byte[] bytes = ImageConversion.EncodeToPNG(tex);
        // byte[] bytes = ImageConversion.EncodeToTGA(tex);
        Object.Destroy(tex);
        File.WriteAllBytes(filepath, bytes);
    }
}

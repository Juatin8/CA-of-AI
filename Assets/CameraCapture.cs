using System.Collections;
using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    private bool isCapturing = false;
    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webcamTexture = new WebCamTexture(devices[0].name, captureWidth, captureHeight);
            GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
            StartCoroutine(CaptureRoutine());
        }
    }

    private IEnumerator CaptureRoutine()
    {
        yield return new WaitForSeconds(2); // 等待摄像头启动

        while (true)
        {
            yield return new WaitForSeconds(5); // 每5秒拍一次照
            CaptureAndSave();
        }
    }

    private void CaptureAndSave()
    {
        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();

        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/Photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png", bytes);
    }
}

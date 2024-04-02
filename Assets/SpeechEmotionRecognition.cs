using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro; // 引入TextMeshPro命名空间
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization; // 添加对System.Globalization的引用
using static SpeechEmotionRecognition;
using System.Linq;

public class SpeechEmotionRecognition : MonoBehaviour
{
    // 修改结构体以使用TextMeshProUGUI
    [System.Serializable]
    public struct EmotionUI
    {
        public Slider slider;
        public TextMeshProUGUI label; // 使用TextMeshProUGUI替代Text
        public string name;
    }


    // 定义所有情绪的UI元素
    public EmotionUI[] emotionsUI;


    private string API_URL = "https://api-inference.huggingface.co/models/harshit345/xlsr-wav2vec-speech-emotion-recognition";
    private string token = "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";

    // 用于从外部接收音频文件的字节数组并开始查询过程
    public void StartQueryWithBytes(byte[] audioBytes)
    {
        StartCoroutine(QueryWithBytes(audioBytes));
    }

    IEnumerator QueryWithBytes(byte[] audioBytes)
    {
        UnityWebRequest www = UnityWebRequest.Post(API_URL, UnityWebRequest.kHttpVerbPOST);
        UploadHandlerRaw uhr = new UploadHandlerRaw(audioBytes);
        uhr.contentType = "application/octet-stream";
        www.uploadHandler = uhr;

        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
else
{
    Debug.Log("Response: " + www.downloadHandler.text);
    UpdateSliders(www.downloadHandler.text);
}
    }


public void UpdateSliders(string jsonResponse)
{
    var emotions = JsonConvert.DeserializeObject<List<EmotionScore>>(jsonResponse);

    // 根据分数降序排序情绪
    var sortedEmotions = emotions.OrderByDescending(e => e.score).ToList();

    // 遍历排序后的情绪列表，更新UI
    for (int i = 0; i < sortedEmotions.Count && i < emotionsUI.Length; i++)
    {
        var sortedEmotion = sortedEmotions[i];
        var emotionUI = emotionsUI[i]; // 直接使用索引i，假设emotionsUI已经按照UI中的顺序排列

        emotionUI.slider.value = sortedEmotion.score; // 更新Slider值
        emotionUI.label.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sortedEmotion.label); // 更新标签文字，首字母大写
    }

    // 如果有未使用的UI元素，可以选择隐藏或将其值设置为0
    for (int i = sortedEmotions.Count; i < emotionsUI.Length; i++)
    {
        emotionsUI[i].slider.value = 0; // 重置剩余Slider
        emotionsUI[i].label.text = ""; // 清空标签
    }
}


// 定义用于解析JSON响应的类
[System.Serializable]
public class EmotionScore
{
    public float score;
    public string label;
}
}

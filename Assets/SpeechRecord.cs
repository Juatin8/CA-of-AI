using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecord : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private TextMeshProUGUI text;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    // 在 Unity Inspector 中设置这个引用，或者使用 GetComponent<>() 获取
    public SpeechEmotionRecognition speechEmotionRecognition;

    private void Start()
    {
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        stopButton.interactable = false;

        // 获取 SpeechEmotionRecognition 组件的引用
        speechEmotionRecognition = GetComponent<SpeechEmotionRecognition>();
        if (speechEmotionRecognition == null)
        {
            Debug.LogError("SpeechEmotionRecognition component not found. Please attach it to the same GameObject.");
        }
    }

    private void Update()
    {
        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
    }

    private void StartRecording()
    {
        text.color = Color.white;
        text.text = "Recording...";
        startButton.interactable = false;
        stopButton.interactable = true;
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording()
    {
        var position = Microphone.GetPosition(null); // 获取当前录音位置
        Microphone.End(null); // 停止录音

        if (position <= 0)
        {
            Debug.LogError("Microphone position is less than or equal to zero. Position: " + position);
            return;
        }

        if (clip == null)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }
        
        var samplesLength = position * clip.channels;
        if (samplesLength <= 0)
        {
            Debug.LogError("Calculated samples length is less than or equal to zero. Length: " + samplesLength);
            return;
        }

        var samples = new float[samplesLength];
        if (clip.samples < position)
        {
            Debug.LogError($"AudioClip samples ({clip.samples}) is less than expected position ({position}).");
            return;
        }

        clip.GetData(samples, 0); // 获取录音数据
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        stopButton.interactable = false;

        //  if (speechEmotionRecognition != null)
        //  {
        speechEmotionRecognition.StartQueryWithBytes(bytes);
        startButton.interactable = true;
        text.text = "finished";
        //  }
    }


    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}

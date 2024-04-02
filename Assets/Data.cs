using System;
using UnityEngine;

public class Data : MonoBehaviour
{
    public int OneSceneLength=30;
    public float MoveSpeed = 10f;
    public float WidthBound = 20f;
    public float HeightBound = 6f;
    [HideInInspector] public int playerScore = 0; // ѵ���ܷ�
    [HideInInspector] public string playerName;  // �������
    [HideInInspector] public int TotalSeconds = 0;
    [HideInInspector] public int Minute = 0;
    [HideInInspector] public int Second = 0;
    [HideInInspector] public string PortName = "/dev/tty.usbserial-0001";

    // ----------------- ������д�� ---------------------
    private static Data instance;
    public static Data Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Data>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
    }
}

    public void AddScore()       //��/������
    {
        playerScore += 1;
    }

    public void CalculateTime()
    {
        Second = ((int)TotalSeconds % 60);
        Minute = ((int)TotalSeconds / 60);
    }
    public void CreateData()
    {
        PlayerPrefs.SetInt(playerName, playerScore);
        DateTime dt = DateTime.Now;
        PlayerPrefs.SetString(playerName + "_" + "date", dt.ToString());

        string allPlayers = PlayerPrefs.GetString("AllPlayers");
        string newAllPlayers = allPlayers + playerName + ";";
        PlayerPrefs.SetString("AllPlayers", newAllPlayers);
    }

    private void OnApplicationQuit() //�˳�APP��debug�󱣴�����
    {
        CreateData();
    }


    private string oneData;
    public void PrintAllResult()
    {
        string allPlayers = PlayerPrefs.GetString("AllPlayers");
        string tempDateString = "";
        if(allPlayers != null)
        {
            string[] allPlayersList = allPlayers.Split(';');
            for(int i = 0; i < allPlayersList.Length - 1; i++)
            {
                if(PlayerPrefs.GetString(allPlayersList[i] + "_date", "null") != "null")
                {
                    int thisScore = PlayerPrefs.GetInt(allPlayersList[i]);
                    int thisCognitive = PlayerPrefs.GetInt(allPlayersList[i] + "_cognitive");
                    int thisMode = PlayerPrefs.GetInt(allPlayersList[i] + "_mode");
                    tempDateString = PlayerPrefs.GetString(allPlayersList[i] + "_date");
                    oneData = "player: " + allPlayersList[i] + "; " + "score: " + thisScore.ToString() + "; " + "cognitive: " + thisCognitive.ToString() + "; " + "mode: " + thisMode.ToString() + "; " + "date: " + tempDateString + ";";
                    Debug.Log(oneData);
                }
            }
        }
    }


}

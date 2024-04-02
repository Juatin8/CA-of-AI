using UnityEngine;

public class ProcessData : MonoBehaviour //���������ݸ���MPU6050�ı������
{
    public float a1, a2;

    private SerialPorter sp;
    public string recievedData1, recievedData2;

    private void Start()
    {
        sp = GetComponent<SerialPorter>();
    }

    private void Update()
    {
        SeperateDataStream(sp.receivedData, ref recievedData1, ref recievedData2); 
        ProcessWitData(recievedData1);        
    }



    //-------------------- ������װ---------------------------
    private void SeperateDataStream(string Odata, ref string recievedData1, ref string recievedData2)  //��ͬԴ���������������ݷ���
    {
        string[] dataParts = Odata.Split(':');
        int num;
        if (int.TryParse(dataParts[0], out num))
        {
            num = int.Parse(dataParts[0]);
        }
        else
        {
            Debug.Log("wrong");
        }
        if (num == 1)
            recievedData1 = Odata;
        else if(num == 2)
            recievedData2 = Odata;
    }

    public void ProcessWitData(string data)  //ά���������ݷ���
    {
        string[] dataParts = data.Split(':');
        a1 = float.Parse(dataParts[1]);
        a2 = float.Parse(dataParts[2]);
    }
}
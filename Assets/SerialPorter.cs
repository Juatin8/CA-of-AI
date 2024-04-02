using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialPorter : MonoBehaviour
{
    SerialPort serialPort;
    public string receivedData;
    Thread readThread;  

    void Start()
    {
        string portName = Data.Instance.PortName;
        serialPort = new SerialPort("/dev/tty.usbserial-0001", 9600, Parity.None, 8, StopBits.One);         // ����SerialPort�������ô��ڲ���
        serialPort.ReadTimeout = 1000; // ���ö�ȡ��ʱʱ��

        serialPort.Open();  // �򿪴�������

        readThread = new Thread(ReadSerialData);  //��һ���߳�������������飬��ռ����Ϸ���̡߳�
        readThread.Start();
    }

    void OnDestroy()
    {
        if(serialPort != null && serialPort.IsOpen)  // �رմ�������
        {
            serialPort.Close();
        }

        if(readThread != null && readThread.IsAlive) 
        {
            readThread.Join();
        }
    }

    void ReadSerialData()
    {
        while(serialPort.IsOpen)
        {
            try
            {
                receivedData = serialPort.ReadLine();   // ��ȡ��������
                Debug.Log("Received data: " + receivedData);
            }
            catch(System.Exception e)
            {
                Debug.LogWarning("Serial port read error: " + e.Message);
            }
        }
    }
}
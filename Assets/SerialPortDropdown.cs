using UnityEngine;
using System.IO.Ports;
using TMPro;

public class SerialPortDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown; // ����Dropdown UIԪ��

    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        string[] availablePorts = SerialPort.GetPortNames(); // ��ȡ���õĴ����б�
        dropdown.ClearOptions();   // ���Dropdown�е�ѡ��
        TMP_Dropdown.OptionDataList portOptions = new TMP_Dropdown.OptionDataList();  // ����һ��ѡ���б��Դ洢��������
        foreach(string portName in availablePorts)  // ��ÿ�����õĴ���������ӵ�ѡ���б���
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(portName);
            portOptions.options.Add(option);
        }
        dropdown.options = portOptions.options;  // ��ѡ���б����ø�Dropdown
    }
    public void OnDropdownValueChanged(int index)
    {
        string selectedPortName = dropdown.options[index].text; // ��Dropdown�л�ȡ�û�ѡ��Ĵ�������
        Debug.Log("�û�ѡ��Ĵ����ǣ�" + selectedPortName);  // ��������Խ�selectedPortName���ݸ���Ĵ���ͨ�Ŵ��룬�Ա��ѡ���Ĵ��ڲ��������ݶ�ȡ������
        Data.Instance.PortName = selectedPortName;
    }
}
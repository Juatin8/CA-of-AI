using UnityEngine;
using System.IO.Ports;
using TMPro;

public class SerialPortDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown; // 引用Dropdown UI元素

    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        string[] availablePorts = SerialPort.GetPortNames(); // 获取可用的串口列表
        dropdown.ClearOptions();   // 清空Dropdown中的选项
        TMP_Dropdown.OptionDataList portOptions = new TMP_Dropdown.OptionDataList();  // 创建一个选项列表以存储串口名称
        foreach(string portName in availablePorts)  // 将每个可用的串口名称添加到选项列表中
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(portName);
            portOptions.options.Add(option);
        }
        dropdown.options = portOptions.options;  // 将选项列表设置给Dropdown
    }
    public void OnDropdownValueChanged(int index)
    {
        string selectedPortName = dropdown.options[index].text; // 从Dropdown中获取用户选择的串口名称
        Debug.Log("用户选择的串口是：" + selectedPortName);  // 现在你可以将selectedPortName传递给你的串口通信代码，以便打开选定的串口并进行数据读取操作。
        Data.Instance.PortName = selectedPortName;
    }
}
using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private ProcessData pd;
    private float speed = 2f;

    //public float sensitivity = 0.01f; // �����ȣ����ڵ���λ�Ƶı���

    private Quaternion initialRotation; // ��ʼ��ת
    private Quaternion targetRotation; // Ŀ����ת
    private Vector3 initialPosition;
    private Vector3 targetPosition;    // Ŀ��λ��
    public float horizontalDistance = 5f; // �����ƶ��ľ���


    float widthBound = 5;
    float heightBound = 4;

    private void Start()
    {
        pd = GameObject.Find("SerialPorter").GetComponent<ProcessData>();

        initialRotation = transform.rotation; //��ʼ��ת��
        initialPosition = transform.localPosition;

        targetPosition = new Vector3(initialPosition.x + horizontalDistance, initialPosition.y, initialPosition.z);
    }
    void FixedUpdate()
    {

        Debug.Log("float" + pd.a1);
    }

    private Vector3 velocity = Vector3.zero;
    private Vector3 displacement = Vector3.zero;
    Vector3 accelerometerData;

}
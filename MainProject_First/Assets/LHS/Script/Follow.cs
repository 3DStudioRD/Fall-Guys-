using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� Ÿ���� ����ٴϰ� �ʹ�.
public class Follow : MonoBehaviour
{
    // ī�޶� ���󰡾� �� Ÿ��
    public Transform target;
    // ��ġ ������(������)
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwingPlatform : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 beforePos;
    Vector3 nowPos;

    void Start()
    {
        firstPos = this.gameObject.transform.position;
        Debug.Log(firstPos);
    }

    void Update()
    {

    }

    // �÷��̾ ��ġ�� ��ǥ�� ���� ����
    // �÷��̾ �̵��� ��ǥ�� ���� ����
    // �̵��� ��ŭ �÷����� ȸ���� ��ȭ
}

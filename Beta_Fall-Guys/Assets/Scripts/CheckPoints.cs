using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPoints : MonoBehaviour
{
    // �ǽð����� ���ϴ� ��ŷ�� ǥ���ϰ� �ʹ�
    // �������� �÷��̾� ������ distance�� ���Ѵ�
    // distance�� ���� ����� ������� 1�� ~ 7����� �ִ´�
    // Trigger
    // �ʿ� �Ӽ� : ������, �÷��̾� ��ġ, �Ÿ� 
    public Transform[] Players;
    public Transform dest;
    public GameObject Rank1;
    public GameObject Rank2;
    public GameObject Rank3;
    public GameObject Rank4;
    public GameObject Rank5;
    public GameObject Rank6;
    public GameObject Rank7;

    // distance�� �������� ������ ���� list�� ���� -> 
    void Update()
    {
        Players = Players.OrderBy((dest) => (dest.position - transform.position).sqrMagnitude).ToArray();
        // Debug.Log(Players[i].name.ToString();
    }
    // Start is called before the first frame update
    /*void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }
        else if(other.CompareTag("Player"))
        {
            // distance�� ���� ���� ���� ����
            // ���� ���� ���� ���� �÷��̾� �̸� ����
            // 1�� �ڸ��� �÷��ֱ�
            float distance = Vector3.Distance(playerTransform.position, dest.position);

            for(int i = 0; i < Players.Length; i++)
            {
                Players[i] = 
                // sorting ��� : ���� ����ϴ� Ŭ����, ����Ʈ 2��, �Ÿ��� ����, ��������/��������
                // ��ųʸ� ����Ʈ
            }
            
        }

        // if(transform == playerTransform.GetComponent)*/
}

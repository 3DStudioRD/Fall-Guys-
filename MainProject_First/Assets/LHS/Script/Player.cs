using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է°��� ���� �¿�յڷ� �̵��ϰ� �ʹ�.
// shiftŰ�� ������ ���� �޸��� �ʹ�.
// jumpŰ�� ������ �ٰ� �ʹ�.
public class Player : MonoBehaviour
{
    // �̵��ӵ�
    public float speed = 10;
    // �����޸��� �ӵ�
    public float runSpeed = 2f;
    // ���� �Ŀ�
    public float jumpPower = 5;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    Animator anim;
    Rigidbody rigid;

    // ����
    bool jDown;
    bool isJump;
    // ���� �޸���
    bool rDown;
    // �߰��׼�
    //bool isVictory;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        //Victory();


    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButton("Jump");
        //shift ��ư�� RunFast �߰���
        // rDown = Input.GetButton("Runfast");
        //isVictory = Input.GetButton("victory");
    }

    void Move()
    {
        // �̵��ϰ�ʹ�
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // ���׿����ڷ� ���� ����
        // bool ���� ���� ? trun �� �� �� : false �϶� ��
        transform.position += moveVec * (rDown ? speed * runSpeed : speed) * Time.deltaTime;

        // Move �ִϸ��̼� true
        anim.SetBool("isMove", moveVec != Vector3.zero);
        // ���� �޸���
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        // �ڿ������� ȸ�� = ���ư��� �������� �ٶ󺻴�
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        // jump�ϰ� �մ� ��Ȳ���� Jump���� �ʵ��� ����
        // ������ �ϰ� ���� �ʴٸ�
        if(jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            
            // Jump Trigger true  ����
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

   /* void Victory()
    {
        anim.SetTrigger("victory");
        isVictory =  true;
    }*/

    // �ٴڿ� ����� �� �ٽ� flase�� �ٲ��ش�. 
    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� �ٴ��̶�� 
        if(collision.gameObject.tag == "envorionment")
        {
            isJump = false;  
        }
    }
}

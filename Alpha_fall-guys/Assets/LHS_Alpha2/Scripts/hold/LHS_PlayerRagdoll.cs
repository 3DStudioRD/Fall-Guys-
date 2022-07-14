using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է°��� ���� �¿�յڷ� �̵��ϰ� �ʹ�.
// jumpŰ�� ������ �ٰ� �ʹ�.
public class LHS_PlayerRagdoll : MonoBehaviour
{
    // �̵��ӵ�
    public float speed = 10;

    // ���� �Ŀ�
    public float jumpPower = 5;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    private Camera currentCamera;
    public bool UseCameraRotation = true;

    Animator anim;
    //Rigidbody rigid;

    // ����
    bool jDown;
    bool isJump;
    bool isDie;

    //************** ���׵� *************//
    // ���� ��ǥ ����
    // Bones Details
    private Rigidbody[] bones;
    private Quaternion[] rotations;

    // Hips
    private Vector3 hipsPosition;
    public Transform hips;

    // Animator
    private Animator animator;

    // ���׵� ����
    private bool activateRagdoll;
    public float rotationSpeed = 3, movementSpeed = 0.33f;

    // Start is called before the first frame update
    void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentCamera = FindObjectOfType<Camera>();

        //************** ���׵� *************//
        // ��� ���� ������ �����Ͽ� ����
        // �ڽ� ������ٵ� �����ͼ� ��� ���� �ִ´�.
        bones = GetComponentsInChildren<Rigidbody>();

        // ȸ��
        rotations = new Quaternion[bones.Length];

        // ����� Animator ������Ұ� �ִ��� Ȯ���� ��
        // �ش� ������ҿ� Animator ������ �Ҵ�
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Expression();
        //Die();
        EnableRagdoll();
        DisableRagdoll();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButton("Jump");
    }

    void Move()
    {
        // ����
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //ī�޶� �������� �����ش�.
        if (UseCameraRotation)
        {
            //ī�޶��� yȸ���� ���ؿ´�.
            Quaternion v3Rotation = Quaternion.Euler(0f, currentCamera.transform.eulerAngles.y, 0f);
            //�̵��� ���͸� ������.
            moveVec = v3Rotation * moveVec;
            //Debug.Log(currentCamera.transform.eulerAngles.y.ToString());
        }

        transform.position += moveVec * speed * Time.deltaTime;

        // Move �ִϸ��̼� true
        anim.SetBool("isMove", moveVec != Vector3.zero);
        
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
            //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    /*void Die()
    {
        if (isDie)
        {
            //rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetTrigger("doDie");
            isDie = true;
        }
    }*/

    // �ٴڿ� ����� �� �ٽ� flase�� �ٲ��ش�. 
    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� �ٴ��̶�� 
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.collider.tag == "Wall")
        {
            //anim.SetTrigger("doDie");
            //isDie = false;
           

        }
    }

    // ����ǥ��
    void Expression()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("doDance01");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("doDance02");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("doVictory");
        }
    }

    //************** ���׵� *************//
    public void EnableRagdoll()
    {
        UpdateRagdollBones();
        activateRagdoll = true;
        // 0�� �� �ִϸ����� ��� ����
        if (animator)
            StartCoroutine(ToggleAnimator(false, 0));
        else
            Debug.LogWarning("There's no Animator component assigned.");
    }

    public void DisableRagdoll()
    {
        activateRagdoll = false;
        // 1.5�ʿ��� �ִϸ����� ���
        if (animator)
            StartCoroutine(ToggleAnimator(true, 1.5f));
        else
            Debug.LogWarning("There's no Animator component assigned.");
    }

    private void UpdateRagdollBones()
    {
        // hips ��ġ�� ���� ĳ���� ��ġ�� ����
        hipsPosition = transform.position;
        // �׸��� ���� ������ ��ġ���� y��ġ�� ����ϴ�.
        hipsPosition.y = hips.position.y;
        // Update the rotations array // ȸ�� �迭 ������Ʈ
        for (int i = 0; i < bones.Length; i++)
            rotations[i] = bones[i].transform.rotation;
    }

    private IEnumerator ToggleAnimator(bool actv, float time)
    {
        // �ð��� ��ٸ� ���� ���� �ִϸ����͸� actv�� ����
        // Wait for "time" seconds and then set animator to "actv"
        yield return new WaitForSeconds(time);
        animator.enabled = actv;
    }

    private void FixedUpdate()
    { 
        if (activateRagdoll)
        {
            // ���׵� ȿ���� Ȱ��ȭ�Ϸ��� ��� ��ݿ��� Kinematic�� false�� ����
            // Set isKinematic to false in all bones to activated the Ragdoll effect
            for (int i = 0; i<bones.Length; i++)
                if (bones[i].isKinematic)
                    bones[i].isKinematic = false;
        }
        // ���׵��� ��Ȱ��ȭ�Ǹ� iskinematic ����� Ȱ��ȭ
        else
        {
             for (int i = 0; i < bones.Length; i++)
             {
                 // �߷� �� �浹�� ������ ���� �ʵ��� �� ���� Ű�׸�ƽ�� ������ �����մϴ�.
                 // Set each bone's isKinematic to true so it won't be affected by gravity and collision
                 if (!bones[i].isKinematic)
                 bones[i].isKinematic = true;
                 // �׷� ���� �� ���� ȸ���� ���� ȸ������ ������Ʈ�մϴ�.
                 // Then we update the rotation of each bone to its previous rotation
                 bones[i].transform.rotation = Quaternion.Lerp(bones[i].transform.rotation, rotations[i], Time.deltaTime * rotationSpeed);
                 // �׸��� �����̸� ���׵� �� ������ ��ġ�� �Ű��ּ���
                 // Ȱ��ȭ �� ������ ��ǥ�� �� ȸ���� ������ ��ġ�� ����
                 // And move the hips to the last position before Ragdoll
                 hips.position = Vector3.MoveTowards(hips.position, hipsPosition, Time.deltaTime * movementSpeed);
             }
        }
    }

    // ���׵��� Ȱ���������� �������
    // Public method to check if the Ragdoll is active
    public bool isRagdollActive()
    {
        return this.activateRagdoll;
    }
}






using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_RagdollController : MonoBehaviour
{
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

    // ��� ���� ������ �����Ͽ� ����
    void Start()
    {
        // �ڽ� ������ٵ� �����ͼ� ��� ���� �ִ´�.
        bones = GetComponentsInChildren<Rigidbody>();

        // ȸ��
        rotations = new Quaternion[bones.Length];
        
        // ����� Animator ������Ұ� �ִ��� Ȯ���� ��
        // �ش� ������ҿ� Animator ������ �Ҵ�
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    // ragdoll ����� Ȱ��ȭ �� ��Ȱ��ȭ �ϴ� �ΰ��� ���
    // Ȱ��ȭ �Ҷ����� UpdateRagdollBone�� ȣ���Ͽ� ���� ������ ������Ʈ

    [ContextMenu("EnableRagdoll")]
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

    [ContextMenu("DisableRagdoll")]
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

    // ���� ������Ʈ ��ɿ��� ���׵��� Ȱ��ȭ�Ǿ��ִ��� Ȯ���ϰ�
    // isKinematic �Ӽ��� ��Ȱ��ȭ�մϴ�.
    void FixedUpdate()
    {
        if (activateRagdoll)
        {
            // ���׵� ȿ���� Ȱ��ȭ�Ϸ��� ��� ��ݿ��� Kinematic�� false�� ����
            // Set isKinematic to false in all bones to activated the Ragdoll effect
            for (int i = 0; i < bones.Length; i++)
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

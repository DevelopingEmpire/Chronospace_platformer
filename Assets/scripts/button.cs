using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    public GameObject mesh;// ��ư �κ� �޽� 

    private void OnTriggerEnter(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // ���� �� �ִ� �ֵ��� �� ĳ.�� �������� 

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (controller != null)
        {
            // �����ٸ�~ 
            // mesh �����̴� ��� ������! 
            mesh.transform.DOLocalMoveY(0f, 0.1f);

            // ������ ���� �ϱ�. �����ų�.. ��.. 
            Debug.Log("����!");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // ���� �� �ִ� �ֵ��� �� ĳ.�� �������� 

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (controller != null)
        {
            // �����ٸ�~ 
            // mesh �����̴� ��� ������! 
            mesh.transform.DOLocalMoveY(0.1f, 0.1f);
            // ������ ���� �ϱ�. �����ų�.. ��.. 
            Debug.Log("����!");
        }
    }
}

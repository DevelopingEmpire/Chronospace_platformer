using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween ��� 

public class WindKeyItem : MonoBehaviour
{
    public GameObject windKeyMesh; //�ڽ��� mesh. ȸ�� ��ų ��� 
                                   // Start is called before the first frame update

    private void OnEnable() // Ȱ��ȭ �� �� 
    {
        Debug.Log("�¿� Ȱ��ȭ");
        //DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // ���� ���ڴµ� �ʱ�ȭ��� ��. �⺻ �����̴� �Ƚᵵ �� 

        windKeyMesh.transform.DOLocalRotate(new Vector3(0, 90, 0), 1f).SetLoops(4, LoopType.Incremental);// ȸ����, ���ӽð� .
        Debug.Log("�¿� ȸ��");
        Invoke("SetActiveFalse", 4.5f); // 7�� �� ��Ȱ��ȭ ��. dotween �� �ڷ�ƾ���� ���� ��.. �Ͽ� �׳� ������ �������ڸ��� ��Ȱ�� 
    }


    void SetActiveFalse() // ������ ��Ȱ��ȭ �ϴ� �Լ� 
    {
        Debug.Log("�¿� ��Ȱ");
        gameObject.SetActive(false);// ������ ������ ���� ���� 
    }

}

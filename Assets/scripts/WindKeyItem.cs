using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween ��� 

public class WindKeyItem : MonoBehaviour
{
    public GameObject windKeyMesh; //�ڽ��� mesh. ȸ�� ��ų ��� 
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // ���� ���ڴµ� �ʱ�ȭ��� ��. �⺻ �����̴� �Ƚᵵ �� 

        windKeyMesh.transform.DOLocalRotate(new Vector3(0, 90, 0), 2f).SetLoops(3,LoopType.Incremental);// ȸ����, ���ӽð� .

        Invoke("SetActiveFalse", 7f); // 7�� �� ��Ȱ��ȭ �� 
    }

    void SetActiveFalse() // ������ ��Ȱ��ȭ �ϴ� �Լ� 
    {
        gameObject.SetActive(false);// ������ ������ ���� ���� 
    }

}

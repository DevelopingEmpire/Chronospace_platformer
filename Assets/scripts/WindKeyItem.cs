using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween ��� 

public class WindKeyItem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // ���� ���ڴµ� �ʱ�ȭ��� ��. �⺻ �����̴� �Ƚᵵ �� 

        transform.DORotate(new Vector3(90, 90, -90), 2f).SetLoops(3,LoopType.Incremental); // ȸ����, ���ӽð� .

    }

}

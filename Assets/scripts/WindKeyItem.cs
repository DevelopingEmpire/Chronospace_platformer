using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class WindKeyItem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // 먼지 몰겠는데 초기화라고 함. 기본 셋팅이니 안써도 됨 

        transform.DORotate(new Vector3(90, 90, -90), 2f).SetLoops(3,LoopType.Incremental); // 회전각, 지속시간 .

    }

}

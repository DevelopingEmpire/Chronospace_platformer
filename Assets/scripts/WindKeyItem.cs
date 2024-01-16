using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class WindKeyItem : MonoBehaviour
{
    public GameObject windKeyMesh; //자식인 mesh. 회전 시킬 대상 
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // 먼지 몰겠는데 초기화라고 함. 기본 셋팅이니 안써도 됨 

        windKeyMesh.transform.DOLocalRotate(new Vector3(0, 90, 0), 2f).SetLoops(3,LoopType.Incremental);// 회전각, 지속시간 .

        Invoke("SetActiveFalse", 7f); // 7초 뒤 비활성화 됨 
    }

    void SetActiveFalse() // 스스로 비활성화 하는 함수 
    {
        gameObject.SetActive(false);// 스스로 꺼지는 착한 아이 
    }

}

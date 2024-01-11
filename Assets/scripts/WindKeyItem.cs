using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class WindKeyItem : MonoBehaviour
{
    //아이템 코드 긁어옴 
    public enum Type { Gravity, TimeStop, WindKey }; // 중력, 시간, 태엽 
    public Type type;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50); // 먼지 몰겠는데 초기화라고 함. 기본 셋팅이니 안써도 됨 

        transform.DORotate(new Vector3(180, 90, -90), 1f).SetLoops(3,LoopType.Incremental); // 회전각, 지속시간 .


    }

}

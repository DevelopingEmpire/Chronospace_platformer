using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageMechanicsController : MonoBehaviour
{
    public abstract void Trigger(); 
    // 버튼이 눌리면
    // 레이저가 특정 부분에 닿으면.
    public abstract void Exit();
    // 버튼이 종료되면
    // 레이저가 특정 영역에서 벗어나면 

    public abstract int Idx { get; set; }
    
}

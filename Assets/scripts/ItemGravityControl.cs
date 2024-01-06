using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : IGravityControl
{

    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, gravityStrength, 0);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);
    }

}

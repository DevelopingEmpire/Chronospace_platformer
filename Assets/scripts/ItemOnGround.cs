using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : Item
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); // ���ۺ��� ȸ�� ȿ��
    }
}

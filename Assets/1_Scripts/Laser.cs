using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    RaycastHit hit;
    public LineRenderer lr; // 얘가 선을 그어줄거야! 
    public Vector3 newPosition;
    public Vector3 newDir;
    public int laserID; // 인스펙터에서 정해주기  

    public GameObject lastPressedButton;

    private void Update()
    {
        if (StageManager.instance.buttons[laserID]) //이게 켜지면 
        {
            LaserOn();
        }
        else
        {
            // 이전에 버튼에 닿았었다면, 그 버튼 꺼주고 null 
            if (lastPressedButton != null)
            {
                lastPressedButton.GetComponent<ButtonController>().OnButtonUp(); // 눌린거 꺼주기 
                lastPressedButton = null;
            }
            lr.positionCount = 0;
        }
    }
     

    public void LaserOn()
    {
        List<Vector3> positions = new List<Vector3>(); // 그릴 점들 리스트 

        newPosition = transform.position;
        newDir = transform.forward;

        positions.Add(newPosition);

        while (true)
        {
            Physics.Raycast(newPosition, newDir, out hit);
            positions.Add(hit.point);
            if (hit.collider.gameObject.CompareTag("mirror"))
            {
                newPosition = hit.point;
                newDir = Vector3.Reflect(newDir, hit.normal); // 반사! 
            }
            else
            {
                //버튼에 닿았다면!! 
                if (hit.collider.gameObject.CompareTag("LaserButton") && hit.collider.gameObject.GetComponent<ButtonController>())
                {
                    lastPressedButton = hit.collider.gameObject;
                    lastPressedButton.GetComponent<ButtonController>().OnButtonPressed(); // 버튼 누르기 
                }
                // 버튼에 닿지 않았다면~ 
                else
                {
                    // 이전에 버튼에 닿았었다면, 그 버튼 꺼주고 null 
                    if (lastPressedButton != null)
                    {
                        lastPressedButton.GetComponent<ButtonController>().OnButtonUp(); // 눌린거 꺼주기 
                        lastPressedButton = null;
                    }

                }
                break;
            }

        }
        lr.positionCount = positions.Count; // 정점 추가 
        for (int i = 0; i < positions.Count; i++)
        {
            lr.SetPosition(i, positions[i]);
        }
    }
}

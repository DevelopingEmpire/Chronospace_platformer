using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static UIManager instance;  // Singleton instance

    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public GameObject startScreen;
    public GameObject settingScreen;
    public GameObject battleHUDScreen;
    public GameObject itemLayOut; // 아이템 가진 현황 

    public bool isPause; // 일시정지 상태를 나타낸다 

    private void Update()
    {
        // 뒤로가기 키를 누르면 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            settingScreen.SetActive(isPause);
        }
    }

    public void OnClickStartButton()
    {
        startScreen.SetActive(false);
    }

    public void hasItemUI(Item.Type useItem, bool has)
    {
        Image itemImage = itemLayOut.transform.GetChild((int)useItem).GetComponent<Image>(); // 0중력, 1시간, 2 태엽 
        if (has)
        {
            itemImage.color = Color.white; // 활 
        }
        else
        {
            itemImage.color = Color.gray; // 비활
        }

    }

    public void equipItemUI(Item.Type useItem)
    {
        Image itemFrame = itemLayOut.transform.GetChild(3).GetComponent<Image>();
        if (useItem == Item.Type.Null)
        {
            itemFrame.color = new Color(itemFrame.color.r, itemFrame.color.g, itemFrame.color.b, 0f); // 투명 
        }
        else
        {
            itemFrame.color = new Color(itemFrame.color.r, itemFrame.color.g, itemFrame.color.b, 1f); // 보임
            itemFrame.transform.position = itemLayOut.transform.GetChild((int)useItem).GetComponent<Image>().transform.position; // 0중력, 1시간, 2 태엽

        }
        
    }

}

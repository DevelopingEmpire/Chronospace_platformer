using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
//using UnityEngine.SceneManagement; // 씬 매니지먼트 

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

    [Header("Main UI")]
    public GameObject pauseScreen; // 일시 정지 창
    public GameObject battleHUDScreen; // HUD 전체 

    public GameObject portalInpoText; // 포탈 입장 불가 텍스트 

    public Image fadeImg; // 암전 화면 
    public Image damageFX; // 피격 화면 


    [SerializeField]
    private float fadeDuration; // 암전 시간 

    [Header("NPC Dialogue UI")]
    public GameObject dialogueUI;
    private TextMeshProUGUI dialogueUIName;
    private TextMeshProUGUI dialogueUIContent;
    public GameObject extUI;
    private TextMeshProUGUI dialogueUIExt; 

    [Header("Item")]
    // 아이템 관련
    private GameObject itemLayOut; // 아이템 가진 현황 
    private Image itemFrame;
    private GameObject itemIcons;

    private void Start()
    {
        // 필요한 것들 캐싱 
        itemLayOut = battleHUDScreen.transform.Find("ItemLayOut").gameObject;

        itemFrame = itemLayOut.transform.GetChild(3).GetComponent<Image>();

        itemIcons = itemLayOut.transform.GetChild(4).gameObject;


    }
    public void OnClickEscButton(bool isPause)
    {
        if(pauseScreen){
            AudioManager.instance.PlaySfx(AudioManager.SFX.SFX_UI_ClickSound);

            pauseScreen.SetActive(isPause);
        }
    }

    #region HUD Screen 
    public void hasItemUI(Item.Type useItem, bool has, int slot)
    {
        //Image itemImage = itemLayOut.transform.GetChild((int)useItem).GetComponent<Image>(); // 0중력, 1시간, 2 태엽 
        GameObject itemSlot = itemLayOut.transform.GetChild(slot).gameObject; // 해당 슬롯 

        Image itemImage = itemSlot.GetComponent<Image>(); // 0중력, 1시간, 2 태엽 

        Image itemIcon = itemIcons.transform.GetChild((int)useItem).gameObject.GetComponent<Image>();

        if (has)
        {
            itemImage.color = Color.white; // 활 

            itemIcon.transform.position = itemImage.transform.position; // 위치 옮겨줌 
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f); // 보임

        }
        else
        {
            itemImage.color = Color.gray; // 비활
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 0f); // 투명
        }

        Debug.Log("useItem: " + useItem + ", has:"+ has+", slot: " + slot);

    }

    // 몇 번째 위치가 선택되었는가? 
    public void equipItemUI(Item.Type useItem, int slot) 
    {
        Debug.Log("equipItem 실행");

        if (useItem == Item.Type.Null)
        {
            itemFrame.color = new Color(itemFrame.color.r, itemFrame.color.g, itemFrame.color.b, 0f); // 투명 
        }
        else
        {
            itemFrame.color = new Color(itemFrame.color.r, itemFrame.color.g, itemFrame.color.b, 1f); // 보임
            itemFrame.transform.position = itemLayOut.transform.GetChild(slot).GetComponent<Image>().transform.position; // 0중력, 1시간, 2 태엽
            
        }
        
    }
    #endregion

    public void SetUIForScene(string sceneName)
    {
        if (sceneName == "Stage0")
        {
            // Robby에서 보일 UI 요소 설정
            battleHUDScreen.SetActive(false);
        }
        else
        {
            // Stage에서 보일 UI 요소 설정
            battleHUDScreen.SetActive(true);
        }
    }

    // 화면 암전
    public void ScreenFade(int set, string sceneName)
    {
        var sequence = DOTween.Sequence();

        if (set == 1)
        {
            // 암전 
            sequence.Append(fadeImg.DOFade(1, fadeDuration));

            sequence.AppendCallback(() => {
                //Insert your logic here.
                GameManager.Instance.FadeCallback(sceneName);
            });
        }
        else
        {
            // 암전 해제 
            sequence.Append(fadeImg.DOFade(0, fadeDuration));
        }

        sequence.Play();

    }

    public void PortalImpossible(int stage)
    {

        portalInpoText.SetActive(true); // 켜준다 
        portalInpoText.GetComponent<TextMeshProUGUI>().text = string.Format($"실험실{stage} 접근 권한을 가진 카드가 필요합니다");
        StartCoroutine(PortalInfoTextFade());
     

    }

    private IEnumerator PortalInfoTextFade()
    {
        yield return new WaitForSeconds(2f); // 2초 대기 

        // fade 
        portalInpoText.SetActive(false ); // 끈다 
    }

    // 피격 화면 
    public IEnumerator DmgFX()
    {
        
        damageFX.color = new Color(damageFX.color.r, damageFX.color.g, damageFX.color.b, 1f); // 보임
            
        yield return new WaitForSeconds(0.25f);

        damageFX.DOFade(0, 0.25f);
        
    }
}

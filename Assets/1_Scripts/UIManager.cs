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
    public GameObject mainScreenObj; // 메인스크린 적힌거. 아래것들 부모임. 함 정리하고싶다...
    public GameObject pauseScreen; // 일시 정지 창
    public GameObject battleHUDScreen; // HUD - 아이템, 시간초 

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

    [Header("EndingScene")]
    public GameObject CaptionBoxObj; // 엔딩 씬에서 쓰는 캡션박스 
    public GameObject TitleScreenObj; // 타이틀 씬에서만 씀 

    public GameObject creditObj; // 엔딩 크레딧 글자 
    public float creditScrollDuration = 20f; // 크레딧 스크롤 시간
    public float creditHoldTime = 3f; // 크레딧 끝에서 멈추는 시간

    [Header("팀원 닉네임")]
    [SerializeField] private string seoName = "서주민"; 
    [SerializeField] private string rohName = "노윤상";
    [SerializeField] private string parkName = "박강";

    private void Start()
    {
        // 필요한 것들 캐싱 
        itemLayOut = battleHUDScreen.transform.Find("ItemLayOut").gameObject;

        itemFrame = itemLayOut.transform.GetChild(3).GetComponent<Image>();

        itemIcons = itemLayOut.transform.GetChild(4).gameObject;

        // 게임 시작 시 암전 해제
        var sequence = DOTween.Sequence();
        sequence.Append(fadeImg.DOFade(0, fadeDuration));
    }
    public void OnClickEscButton(bool isPause)
    {
        if (pauseScreen) {
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

        Debug.Log("useItem: " + useItem + ", has:" + has + ", slot: " + slot);

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


            // UIManager.instance.OnClickBattleButton();
            // UIManager.instance.pickUpScreen.SetActive(true);
            // UIManager.instance.selectedStageName = preStageName;
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

    /*
    public void PortalImpossible(int stage)
    {

        portalInpoText.SetActive(true); // 켜준다 
        portalInpoText.GetComponent<TextMeshProUGUI>().text = string.Format($"실험실{stage} 접근 권한을 가진 카드가 필요합니다");
        StartCoroutine(PortalInfoTextFade());
     

    }
    */

    private IEnumerator PortalInfoTextFade()
    {
        yield return new WaitForSeconds(2f); // 2초 대기 

        // fade 
        portalInpoText.SetActive(false); // 끈다 
    }

    // 피격 화면 
    public IEnumerator DmgFX()
    {

        damageFX.color = new Color(damageFX.color.r, damageFX.color.g, damageFX.color.b, 1f); // 보임

        yield return new WaitForSeconds(0.25f);

        damageFX.DOFade(0, 0.25f);

    }

    // main화면 모두 끄기. 엔딩에서 씀
    public void SetMainUIActive(bool set)
    {
        mainScreenObj.SetActive(set);
    }

    // 엔딩씬에서만 씀 
    public void SetCaptionBoxActive(bool set)
    {
        CaptionBoxObj.SetActive(set);
    }

    public GameObject GetCaptionBox()
    {
        return CaptionBoxObj;
    }

    // 엔딩 크레딧 설정 밑 올리기 
    public void EndingCreditStart()
    {
        // 크레딧 텍스트 설정
        TextMeshProUGUI creditText = creditObj.GetComponent<TextMeshProUGUI>();
        creditText.text = GetCreditText();

        // 초기 위치 설정 (화면 아래에서 시작)
        RectTransform creditTransform = creditObj.GetComponent<RectTransform>();
        creditTransform.anchoredPosition = new Vector2(0, -Screen.height);

        // 크레딧 스크롤 애니메이션 (위로 이동)
        creditTransform.DOAnchorPosY(Screen.height, creditScrollDuration).SetEase(Ease.Linear);

    }

    // 크레딧 텍스트를 동적으로 설정
    private string GetCreditText()
    {
        return $@"
        <b>Credit</b>

        <b>Project Leader</b>
        {rohName}


        <b>Programming Development</b>
        {parkName}
        {seoName}


        <b>Design</b>
        {parkName}
        {seoName}
        {rohName}

        <b>Modeling</b>
        {parkName}


        <b>Level Design</b>
        {parkName}
        {seoName}


        <b>Art works</b>
        {seoName}


        <b>BGM</b>
        {parkName}


        <b>Doll Landing Sound</b>
        {seoName}’s Pillow


        <b>Story</b>
        {parkName}


        <b>Quality Assurance / Testing</b>
        {parkName}
        {seoName}


        <b>Publishing & Marketing</b>
        {parkName}
        {seoName}


        <b>Lost in Time</b>
        {rohName}


        <b>Development Period : Dec 2023 - Mar 2025 </b>

        <b>Team DevelopingEmpire</b>
        ";
    }
}

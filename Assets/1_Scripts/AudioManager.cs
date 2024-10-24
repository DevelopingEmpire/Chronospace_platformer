using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static AudioManager instance;  // Singleton instance

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

    public enum AudioType { BGM, SFX }

    [Header("BGM")]
    public AudioClip[] bgmClips; // 배경음악
    public float bgmVolume; // 배경음악 볼륨
    public AudioSource bgmPlayer; // 재생해줄 오디오 소스 컴포넌트 
    public float fadeDuration = 2.0f;  // 페이드 아웃에 걸리는 시간

    public enum BGM
    {
        BGM_Title = 0,
        BGM_Lobby = 1,
        BGM_InStage = 2
    }

    /*
     *    public enum BGM
    {
        BGM_Lobby = 0,
        BGM_NormalStage1 = 1,
        BGM_NormalStage4 = 2,
        BGM_NormalStage5 = 3,
        BGM_NormalStage7 = 4,
        BGM_NormalStage8 = 5,
        BGM_NormalStage10 = 6,
        BGM_ChaosStage1 = 7,
        BGM_ChaosStage4 = 8,
        BGM_ChaosStage5 = 9,
        BGM_ChaosStage7 = 10,
        BGM_ChaosStage8 = 11,
        BGM_ChaosStage10 = 12,
    }
     */

    [Header("SFX")]
    public AudioClip[] sfxClips; // 효과음
    public float sfxVolume; // 효과음 볼륨
    public int channels =5 ; 
    public AudioSource[] sfxPlayers;
    int channelIndex; //


    public enum SFX
    {
        // UI
        SFX_UI_ClickSound ,         //0

        // Basic actions
        SFX_PlayerDmgSound ,        //1
        SFX_PlayerWalkSound ,       //2
        SFX_PlayerDieSound ,        //3
        SFX_RespawnSound ,          //4
        SFX_JumpSound ,             //5
        SFX_ItemEquipSound ,        //6
        SFX_TimerTickSound ,        //7

        // interactions
        SFX_ItemUseSound,           //8
        SFX_ButtonPressed ,         //9
        SFX_EnterPortal ,           //10
        SFX_SwitchPressed ,         //11
        SFX_InteractionActivateSound,   //12
        SFX_DoorOpen,               // 13
        SFX_VendingMachineWork,     //14

        // enemy
        SFX_DetectionSound ,        //15
        SFX_FireSound ,             //16

        // 디폴트
        SFX_None                    //17
    }

    public float BGMVolume
    {
        get => GetVolume(AudioType.BGM);
        set => OnVolumeChanged(AudioType.BGM, value);
    }

    public float SFXVolume
    {
        get => GetVolume(AudioType.SFX);
        set => OnVolumeChanged(AudioType.SFX, value);
    }

    private void Start()
    {
        Init();

        Scene scene = SceneManager.GetActiveScene(); //함수 안에 선언하여 사용한다.

        if (scene.name == "Title")
        {
            PlayBgm(BGM.BGM_Title);
        }
        else if(scene.name == "Stage0")
        {
            PlayBgm(BGM.BGM_Lobby);
        }
        else
        {
            PlayBgm(BGM.BGM_InStage);
        }
    }

    // 초기화 
    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;                          // 게임 시작 시 재생 끄기

        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        // 용량 최적화
        bgmPlayer.dopplerLevel = 0.0f;
        bgmPlayer.reverbZoneMix = 0.0f;
        //bgmPlayer.clip = bgmClips;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            sfxPlayers[idx] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[idx].playOnAwake = false;
            sfxPlayers[idx].volume = sfxVolume;
            sfxPlayers[idx].dopplerLevel = 0.0f;
            sfxPlayers[idx].reverbZoneMix = 0.0f;
        }

        bgmVolume = 1.0f - PlayerPrefs.GetFloat("BGM_Volume");           // default 값이 0이기 때문에 1.0f - value로 저장
        sfxVolume = 1.0f - PlayerPrefs.GetFloat("Effect_Volume");
    }

    public void PlayBgm(BGM bgm)
    {
        if (bgmPlayer == null) return;
        bgmPlayer.clip = bgmClips[(int)bgm];
        bgmPlayer.Play();
    }

    public void StopBgm()
    {
        if (bgmPlayer != null) 
        {
            // bgmPlayer.Stop();

            // 페이드 아웃 
            bgmPlayer.DOFade(0f, fadeDuration).OnComplete(() => bgmPlayer.Stop());  // 볼륨을 0으로 줄이고 종료
        }

    }

    

    public void PlaySfx(SFX sfx)
    {
        // 쉬고 있는 하나의 sfxPlayer에게 clip을 할당하고 실행
        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIndex = (idx + channelIndex) % sfxPlayers.Length;    // 채널 개수만큼 순회하도록 채널인덱스 변수 활용

            if (sfxPlayers[loopIndex].isPlaying) continue;               // 진행 중인 sfxPlayer는 쭉 진행

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // UI 클릭 음 재생 
    public void PlayClickSound()
    {
        PlaySfx(SFX.SFX_UI_ClickSound);  // 특정 SFX를 재생
    }


    public void OnChangedBGMVolume(float value)
    {
        BGMVolume = value;
        bgmPlayer.volume = BGMVolume;
    }

    public float GetVolume(AudioType type)
    {
        return type == AudioType.BGM ? bgmPlayer.volume : sfxPlayers[0].volume;
    }

    public void OnVolumeChanged(AudioType type, float value)
    {
        PlayerPrefs.SetFloat(type == AudioType.BGM ? "BGM_Volume" : "SFX_Volume", 1.0f - value);

        if (type == AudioType.BGM)
        {
            bgmPlayer.volume = value;
        }
        else
        {
            foreach (var player in sfxPlayers)
            {
                player.volume = value;
            }
        }
    }

}

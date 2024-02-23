using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    [Header("BGM")]
    public AudioClip bgmClip; // 배경음악
    public float bgmVolume; // 배경음악 볼륨
    public AudioSource bgmPlayer; // 재생해줄 오디오 소스 컴포넌트 

    [Header("SFX")]
    public AudioClip[] sfxClips; // 효과음
    public float sfxVolume; // 효과음 볼륨
    //public int channels; 
    public AudioSource[] sfxPlayers;
    int channelIndex; //

    public enum Sfx { Coin, levelUp } // 나중에 추가 수정 

    public void playBgm(bool bgmPlay)
    {
        if (bgmPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) // 효과음 플레이어 개수만큼 for문 돈다 
        {
            int loopIndex =  (index + channelIndex) % sfxPlayers.Length; // 마지막 재생한 플레이어 부터 탐색한다 . 갯수 넘어가면 0부터 다시 

            if (sfxPlayers[loopIndex].isPlaying) 
            {
                continue;
            }

            // 재생중이 아니라면 
            channelIndex = loopIndex; // 마지막으로 재생한 플레이어를 기억해둔다
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break; // 재생 후에 종료한다 
        }
    }
}

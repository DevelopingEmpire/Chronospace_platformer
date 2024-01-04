using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class ItemEffect : MonoBehaviour
{
    //�÷��̾�ν� ������ ���ӿ�����Ʈ�� ������ �ε���
    public int itemIndex;

    //ȿ���� �޴� �÷��̾�
    public GameObject targetPlayer;
    public GameObject targetPlayer2;

    //������ ���� ȿ�� ���
    public UnityEvent[] itemEffectList1;
    public UnityEvent[] itemEffectList2;
    //public float[] itemEffectDur1;
    //public float[] itemEffectDur2;

    //������ ȿ�� ���� ����
    public float timeScaleMultiplier = 8.0f;

    private Player targetScript;

    // Start is called before the first frame update
    void Start()
    {
        if (targetPlayer != null)
        {
            Player targetScript = targetPlayer.GetComponent<Player>();
            if (targetScript = null)
            {
                Debug.LogError("Character script not found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("Character GameObject not found.");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        itemIndex = (targetPlayer.GetComponent<Player>()).itemIndex;
    }

    public void UseItem1 ()
    {
        // Check if the itemIndex is within the valid range
        if (itemIndex >= 0 && itemIndex < itemEffectList1.Length)
        {
            // Invoke the UnityEvent at the specified index
            itemEffectList1[itemIndex]?.Invoke();
        }
        else if (itemIndex==-1)
        {
            Debug.Log("No Item Holding Mode.");
        }
        else
        {
            Debug.LogError("Invalid itemIndex: " + itemIndex);
        }
    }

    //Function of Items list
    public void Blank ()
    {
        //do nothing
    }

    public void Tweaktime ()
    {
        Time.timeScale = timeScaleMultiplier;
        Invoke("Tweaktime_End", 10f);
        Debug.Log("Time Scale Tweaked.");
    }
    public void Tweaktime_End()
    {
        Time.timeScale = 1.0f;
    }
}

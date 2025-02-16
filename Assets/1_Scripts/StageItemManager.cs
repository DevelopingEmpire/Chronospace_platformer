using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItemManager : MonoBehaviour
{
    [Header("Universal")]
    [SerializeField] GameObject[] instanceTargetItem; //무조건 5개로 구성, { Gravity, TimeStop, Magneticgrav, Shield, WindKey, Null } 순서로 배치

    [Header("Varies on stage")]
    public List<Item.Type> itemOnField;
    public List<Vector3> itemOnFieldLocation;
    private GameObject[] itemOnFieldArray;
    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SummerizeItem), 1f);
        Invoke(nameof(AppealToPlayer), 1f);
        //Invoke(nameof(ReInstanciateDestroyedItem), 12f);
    }

    void SummerizeItem()
    {
        itemOnFieldArray = GameObject.FindGameObjectsWithTag("Item");
        Debug.Log("Item Count: " + itemOnFieldArray.Length);

        for(int i = 0; i < itemOnFieldArray.Length; i++){
            itemOnFieldLocation.Add(itemOnFieldArray[i].transform.position);
            itemOnField.Add(itemOnFieldArray[i].GetComponent<Item>().type);
        }
    }

    void AppealToPlayer(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.SetSim(this);
    }

    public void ReInstanciateDestroyedItem()
    {
        for(int i = 0; i < itemOnFieldArray.Length; i++){
            if(!itemOnFieldArray[i]){
                Debug.Log("Item " + i + " has been used.");

                switch(itemOnField[i]){
                    case Item.Type.Gravity:
                        Instantiate(instanceTargetItem[0], itemOnFieldLocation[i], Quaternion.identity);
                        break;
                    case Item.Type.TimeStop:
                        Instantiate(instanceTargetItem[1], itemOnFieldLocation[i], Quaternion.identity);
                        break;
                    case Item.Type.Magneticgrav:
                        Instantiate(instanceTargetItem[2], itemOnFieldLocation[i], Quaternion.identity);
                        break;
                    case Item.Type.Shield:
                        Instantiate(instanceTargetItem[3], itemOnFieldLocation[i], Quaternion.identity);
                        break;
                    case Item.Type.WindKey:
                        Instantiate(instanceTargetItem[4], itemOnFieldLocation[i], Quaternion.identity);
                        break;
                    default:
                        Debug.LogError("This type of item is not able to be instanciated");
                        break;
                }
            }
        }
    }
}

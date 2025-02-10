using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LobbyConditonalSwitchController : MonoBehaviour
{
    public string path;
    private string loadedJson;
    private SaveData saveData;
    //public List<string> stageName;

    [SerializeField] StageMechanicsController targetObjectNoncondition; //자동적으로 열 수 있도록 첫 스테이지의 출입문을 할당
    [SerializeField] List<StageMechanicsController> targetObjects;
    [SerializeField] List<int> stageRequiredLevels;
    private InteractionObject targetFuncScript;

    [Header("Material Changes")]
    public Color selfColor;
    public Material selfRefMaterial;
    public Material selfRefMaterialGlow;
    private Material newMaterial;
    private Material newGlowMaterial;

    // Start is called before the first frame update
    void Start()
    {
        LoadDataPath();

        RecolorMaterials();
        RecolorTargetObject();

        // Check stages mentioned in stageRequiredLevels
        /*
        Debug.Log("Stage Required Levels and its status:");
        for(int i = 0; i < stageRequiredLevels.Count; i++){
            Debug.Log(i + " - Stage " + stageRequiredLevels[i] + " clear status: " + saveData.stageClearStatus[stageRequiredLevels[i]] + " affects: " + targetObjects[i]);
        }
        */

        Invoke(nameof(DoActivationCheckout), 1f);
    }

    // This is called after savedata is loaded
    public void DoActivationCheckout(){
        targetObjectNoncondition.Trigger();

        for(int i = 0; i < stageRequiredLevels.Count; i++){
            Debug.Log(i + " - Stage " + stageRequiredLevels[i] + ", affects: " + targetObjects[i] + ", clear status of " + stageRequiredLevels[i] + " is: " + saveData.stageClearStatus[stageRequiredLevels[i]]);
            if(saveData.stageClearStatus[stageRequiredLevels[i]]){
                targetObjects[i].Trigger();
                Debug.Log(targetObjects[i] + " has Activated.");
            }
        }
    }

    void LoadDataPath(){
        loadedJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, "StageData.json"));
        saveData = JsonUtility.FromJson<SaveData>(loadedJson);
        if(saveData != null){
            Debug.Log("Success to load saved data, Stage Count: " + saveData.stageClearStatus.Count);
        }
    }

    void RecolorMaterials()
    {
        if(selfRefMaterial){
            newMaterial = new Material(selfRefMaterial);
            newMaterial.color = selfColor;
        }

        if(selfRefMaterialGlow){
            newGlowMaterial = new Material(selfRefMaterialGlow);
            newGlowMaterial.color = selfColor;
            newGlowMaterial.SetColor("_EmissionColor", selfColor); // Set emission color
        }

    }

    void RecolorTargetObject(){
        foreach (StageMechanicsController targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                targetFuncScript = targetObject.GetComponent<InteractionObject>();
                targetObject.SetInitialColor(newMaterial, newGlowMaterial);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCImage : MonoBehaviour
{
    [Header("Material Changes")]
    public GameObject[] selfMesh; // 게임 오브젝트 배열
    public int[] selfRecoloredMaterials; // 머티리얼 인덱스 배열
    public Texture2D targetImage; // 적용할 텍스처

    public float brightness = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ApplyTextureToMaterials();
    }

    void ApplyTextureToMaterials()
    {
        // 모든 게임 오브젝트에 대해 루프
        for (int i = 0; i < selfMesh.Length; i++)
        {
            Renderer renderer = selfMesh[i].GetComponent<Renderer>();

            if (renderer != null && targetImage != null)
            {
                Material[] materials = renderer.materials; // 오브젝트의 모든 머티리얼 배열

                // selfRecoloredMaterials 배열의 인덱스를 초과하지 않는지 확인
                if (selfRecoloredMaterials[i] < materials.Length)
                {
                    Material targetMaterial = materials[selfRecoloredMaterials[i]]; // 특정 인덱스의 머티리얼

                    //                    targetMaterial.SetColor("_Color", targetImage); // 텍스처 적용
                    targetMaterial.mainTexture = targetImage; // 텍스처 적용
                    targetMaterial.SetTexture("_EmissionMap",targetImage);

                    // 변경된 머티리얼을 다시 설정
                    materials[selfRecoloredMaterials[i]] = targetMaterial;
                    renderer.materials = materials;
                }
                else
                {
                    Debug.LogWarning($"selfRecoloredMaterials 인덱스 {selfRecoloredMaterials[i]}가 {selfMesh[i].name}의 머티리얼 수를 초과합니다.");
                }
            }
            else
            {
                Debug.LogWarning($"{selfMesh[i].name}에 Renderer 컴포넌트가 없습니다.");
            }
        }
    }
}

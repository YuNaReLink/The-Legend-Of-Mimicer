using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトのマテリアルを変更することに使用
/// 現在はスイッチオブジェクトに使用
/// </summary>
public class ColorChange : MonoBehaviour
{
    public enum MaterialTag
    {
        Null = -1,
        One,
        Two,
        DataEnd
    }

    [SerializeField]
    private List<Material>  materialsArray = new List<Material>();
    [SerializeField]
    private MeshRenderer    meshRenderer = null;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeMaterial(MaterialTag tag)
    {
        if(meshRenderer.material == materialsArray[(int)tag]) { return; }
        meshRenderer.material = materialsArray[(int)tag];
    }
}

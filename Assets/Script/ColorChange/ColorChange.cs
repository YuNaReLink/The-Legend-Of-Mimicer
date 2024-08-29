using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̃}�e���A����ύX���邱�ƂɎg�p
/// ���݂̓X�C�b�`�I�u�W�F�N�g�Ɏg�p
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

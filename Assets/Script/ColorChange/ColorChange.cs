using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Material> materialsArray = new List<Material>();
    [SerializeField]
    private MeshRenderer meshRenderer = null;

    // Start is called before the first frame update
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

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�I�u�W�F�N�g�ɃA�^�b�`����Renderer�����邾���擾�A�ێ����Ďg���N���X
/// </summary>
public class RendererData : MonoBehaviour
{
    [SerializeField]
    private List<Renderer>          rendererList = new List<Renderer>();

    public List<Renderer>           RendererList {  get { return rendererList; } set { rendererList = value; } }
    /// <summary>
    /// �I�u�W�F�N�g�̌��̃}�e���A����ێ�����List
    /// </summary>
    [SerializeField]
    private List<Color>             originalColorList = new List<Color>();
    public IReadOnlyList <Color>    GetOriginalColorList() { return originalColorList; }

    public void AwakeInitilaize()
    {
        GameObject g = null;
        Renderer r = null;
        Color c = Color.white;
        for(int i = 0; i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            if (g.GetComponent<Renderer>() == null) { continue; }
            r = g.GetComponent<Renderer>();
            rendererList.Add(r);
            for(int j = 0; j < r.materials.Length; j++)
            {
                c = r.materials[j].color;
                originalColorList.Add(c);
            }
        }
    }
}

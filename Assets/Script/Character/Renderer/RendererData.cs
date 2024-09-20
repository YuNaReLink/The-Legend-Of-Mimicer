using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターオブジェクトにアタッチしてRendererをあるだけ取得、保持して使うクラス
/// </summary>
public class RendererData : MonoBehaviour
{
    [SerializeField]
    private List<Renderer>          rendererList = new List<Renderer>();

    public List<Renderer>           RendererList {  get { return rendererList; } set { rendererList = value; } }
    /// <summary>
    /// オブジェクトの元のマテリアルを保持するList
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

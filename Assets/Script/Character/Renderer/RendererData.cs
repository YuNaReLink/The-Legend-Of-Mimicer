using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererData : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> rendererList = new List<Renderer>();

    public List<Renderer> GetRendererList() {  return rendererList; }

    public void AwakeInitilaize()
    {
        GameObject g = null;
        Renderer r = null;
        for(int i = 0; i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            if (g.GetComponent<Renderer>() == null) { continue; }
            r = g.GetComponent<Renderer>();
            rendererList.Add(r);
        }
    }
}

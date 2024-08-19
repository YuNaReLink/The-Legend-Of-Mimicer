using UnityEngine;

public class Quiver : MonoBehaviour
{
    private int count = 0;
    public int Count { get { return count; }set { count = value; } }
    private void Start()
    {
        count = 20;
    }
}

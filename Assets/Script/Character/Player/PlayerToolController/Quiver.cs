using UnityEngine;

/// <summary>
/// �N���X�{�E�Ŏg����̃J�E���g���Ǘ����Ă�N���X
/// </summary>
public class Quiver : MonoBehaviour
{
    private const int   MaxArrowCount = 20;
    [SerializeField]
    private int         count = 0;
    public int          Count { get { return count; }set { count = value; } }
    private void Start()
    {
        count = MaxArrowCount;
    }

    public void AddArrow(int _count)
    {
        count += _count;
        if (count > MaxArrowCount)
        {
            count = MaxArrowCount;
        }
    }
}

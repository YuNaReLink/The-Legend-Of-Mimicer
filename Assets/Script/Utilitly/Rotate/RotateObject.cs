using UnityEngine;


/// <summary>
/// 回転させたいオブジェクトにアタッチして使う
/// 回転処理を行うクラス
/// X,Y,Zのboolをtrueにすればtrueの方向に回転する
/// reverseで逆回転も可能
/// </summary>
public class RotateObject : MonoBehaviour
{
    [Header("X方向回転設定")]
    [SerializeField]
    private bool            rotateX = false;
    [Header("Y方向回転設定")]
    [SerializeField]
    private bool            rotateY = false;
    [Header("Z方向回転設定")]
    [SerializeField]
    private bool            rotateZ = false;
    [Header("速度設定")]
    [SerializeField]
    private float           speed = 10f;
    [Header("反転設定")]
    [SerializeField]
    private bool            reverse = false;

    private int Reverse()
    {
        return reverse ? -1 : 1;
    }

    
    private void Update()
    {
        if (rotateX)
        {
            MoveRotate(Vector3.right * Reverse(), speed);
        }

        if(rotateY)
        {
            MoveRotate(Vector3.up * Reverse(), speed);
        }

        if(rotateZ)
        {
            MoveRotate(Vector3.forward * Reverse(), speed);
        }
    }

    private void MoveRotate(Vector3 dir,float _speed)
    {
        transform.Rotate(dir,_speed * Time.deltaTime);
    }
}

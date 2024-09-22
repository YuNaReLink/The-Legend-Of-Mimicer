using UnityEngine;

/// <summary>
/// 剣自身にアタッチするクラス
/// エフェクトや当たり判定は剣自体が制御する考え方
/// </summary>
public class SwordController : ToolController
{
    public override float           AttackPower => base.AttackPower * ratioPower;

    private float                   ratioPower = 1f;

    public override void SetController(CharacterController _controller)
    {
        base.SetController(_controller);
        player = controller.GetComponent<PlayerController>();
    }
    private PlayerController        player = null;

    public override ToolTag GetToolTag() { return ToolTag.Sword; }
    private SwordEffectController   effect = null;

    private CharacterTagList.StateTag[] StateArray = new CharacterTagList.StateTag[]
    {
        CharacterTagList.StateTag.Attack,
        CharacterTagList.StateTag.JumpAttack,
        CharacterTagList.StateTag.SpinAttack,
    };

    /// <summary>
    /// コライダーの中心、サイズを区別するためのState
    /// </summary>
    private enum ColliderType
    {
        Normal,
        SpinAttack
    }
    private BoxCollider boxCollider = null;
    /// <summary>
    /// 状態によって剣のコライダーの中心とサイズを変更する値を保持した配列
    /// </summary>
    private Vector3[] colliderCenter = new Vector3[]
    {
        new Vector3(0,1.1f,0),
        new Vector3(0,1.9f,0)
    };
    private Vector3[] colliderScale = new Vector3[]
    {
        new Vector3(0.6f,1.8f,0.2f),
        new Vector3(1,3.5f,1)
    };
    protected override void Awake()
    {
        base.Awake();
        if (collider != null)
        {
            boxCollider = collider.GetComponent<BoxCollider>();
        }
        else
        {
            Debug.Log("Colliderがアタッチされていない");
        }
        effect = GetComponent<SwordEffectController>();
    }
    private void Start()
    {
        //エフェクトの初期化
        effect.StopTrail();
    }

    void Update()
    {
        ActiveCheck();
        SetAttackPower();
        SetColliderSize();
    }

    /// <summary>
    /// 剣のコライダーを有効にするか決める関数
    /// </summary>
    private void ActiveCheck()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        if (MotionTimeCheck(state))
        {
            effect.PlayTrail();
            collider.enabled = true;
        }
        else
        {
            effect.StopTrail();
            collider.enabled = false;
        }
    }

    private const float JumpattackStartCount = 0.3f;
    private const float JumpattackEndCount = 0.5f;

    private const float SpinAttackStartCount = 0.3f;
    /// <summary>
    /// モーションの名前から現在のモーションを取得して
    /// モーションの再生時間でコライダーを有効にするかを決める
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private bool MotionTimeCheck(CharacterTagList.StateTag tag)
    {
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        PlayerController player = controller.GetComponent<PlayerController>();
        switch (tag)
        {
            case CharacterTagList.StateTag.Attack:
                if (TripleAttackCheck(animInfo))
                {
                    return true;
                }
                break;
            case CharacterTagList.StateTag.JumpAttack:
                if (animInfo.normalizedTime >= JumpattackStartCount &&
                    animInfo.normalizedTime < JumpattackEndCount)
                {
                    return true;
                }
                break;
            case CharacterTagList.StateTag.SpinAttack:
                if (animInfo.normalizedTime < SpinAttackStartCount)
                {
                    return true;
                }
                break;
            default:
                return false;
        }

        return false;
    }

    private const float FirstAttackStartTime = 0.3f;
    private const float FirstAttackEndTime = 0.7f;

    private const float SecondAttackStartTime = 0.1f;
    private const float SecondAttackEndTime = 0.4f;

    private const float ThreeAttackStartTime = 0.5f;
    private const float ThreeAttackEndTime = 0.7f;
    /// <summary>
    /// 三段攻撃時のコライダーを有効にする間隔を決める関数
    /// </summary>
    /// <param name="animInfo">
    /// キャラクターのアニメーターを代入する引数
    /// </param>
    /// <returns></returns>
    private bool TripleAttackCheck(AnimatorStateInfo animInfo)
    {
        PlayerController player = controller.GetComponent<PlayerController>();
        switch (player.TripleAttack)
        {
            case CharacterTagList.TripleAttack.First:
                if (animInfo.normalizedTime >= FirstAttackStartTime &&
                    animInfo.normalizedTime < FirstAttackEndTime)
                {
                    return true;
                }
                break;
            case CharacterTagList.TripleAttack.Second:
                if (animInfo.normalizedTime >= SecondAttackStartTime &&
                    animInfo.normalizedTime < SecondAttackEndTime)
                {
                    return true;
                }
                break;
            case CharacterTagList.TripleAttack.Third:
                if (animInfo.normalizedTime >= ThreeAttackStartTime &&
                    animInfo.normalizedTime < ThreeAttackEndTime)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    /// <summary>
    /// 複数ある攻撃状態からダメージ量を決める関数
    /// </summary>
    private void SetAttackPower()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                ratioPower = statusData.BaseDamagePower;
                break;
            case CharacterTagList.StateTag.JumpAttack:
                ratioPower = player.GetData().StrongAttackPowerRatio1;
                break;
            case CharacterTagList.StateTag.SpinAttack:
                ratioPower = player.GetData().StrongAttackPowerRatio2;
                break;
        }
    }
    /// <summary>
    /// 状態によって剣のコライダーのサイズを変更する関数
    /// </summary>
    private void SetColliderSize()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        if(boxCollider == null) { return; }
        if(controller.CharacterStatus.CurrentState == controller.CharacterStatus.PastState) { return; }
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;
        switch (state)
        {
            case CharacterTagList.StateTag.SpinAttack:
                center = colliderCenter[(int)ColliderType.SpinAttack];
                size  = colliderScale[(int)ColliderType.SpinAttack];
                break;
            default:
                center = colliderCenter[(int)ColliderType.Normal];
                size  = colliderScale[(int)ColliderType.Normal];
                break;
        }
        boxCollider.center = center;
        boxCollider.size = size;
    }

}

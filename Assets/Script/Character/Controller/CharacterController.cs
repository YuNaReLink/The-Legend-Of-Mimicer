using CharacterTagList;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStatus characterStatus;
    public CharacterStatus CharacterStatus => characterStatus;
    /// <summary>
    /// アニメーターインスタンス
    /// </summary>
    protected Animator                      animator = null;
    public Animator                         GetAnimator() { return animator; }

    protected AnimatorOverrideController    animatorOverride;
    public AnimatorOverrideController       GetAnimatorOverrideController() { return animatorOverride; }

    protected MotionController              motion = null;
    public MotionController                 GetMotion() {  return motion; }

    protected virtual void                  SetMotionController(){ motion = new MotionController();}

    ///<summary>
    ///Collider
    ///</summary>
    protected Collider                      characterCollider;
    public Collider                         GetCharacterCollider() { return characterCollider; }
    protected Rigidbody                     characterRB;
    public Rigidbody                        CharacterRB { get { return characterRB; } set { characterRB = value; } }

    /// <summary>
    /// 着地判定
    /// </summary>
    protected GroundCheck                   groundCheck = null;
    
    /// <summary>
    /// エフェクトを管理するクラス
    /// </summary>
    protected EffectController              effectController = null;
    public EffectController                 GetEffectController() { return effectController; }
    /// <summary>
    /// オブジェクトのMeshRendererを取得、保持するクラス
    /// </summary>
    protected RendererData                  rendererData = null;
    public RendererData                     GetRendererData() { return rendererData; }
    /// <summary>
    /// オブジェクトのカラーを変更するクラス
    /// </summary>
    protected RendererEffect                rendererEffect = null;
    public RendererEffect                   GetRendererEffect() { return rendererEffect; }
    /// <summary>
    /// ノックバックの処理を行うクラス
    /// </summary>
    protected KnockBackCommand              knockBackCommand = null;
    public KnockBackCommand                 GetKnockBackCommand() { return knockBackCommand; }

    /// <summary>
    /// キャラクターが戦闘状態かそうじゃないか
    /// </summary>
    [SerializeField]
    private bool battleMode = false;
    public bool BattleMode { get { return battleMode; } set { battleMode = value; } }

    /// <summary>
    /// オブジェクトの表示、非表示を設定する関数3選
    /// </summary>
    /// <returns></returns>
    public GameObject                       SelfObject() { return gameObject; }
    public bool                             IsActiveObject() { return gameObject.activeSelf; }
    public void                             SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }
    protected virtual void Awake(){}
    protected virtual void InitializeAssign()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animatorがアタッチされていません");
        }

        SetMotionController();

        characterCollider = GetComponent<Collider>();
        if(characterCollider == null)
        {
            Debug.Log("Colliderがアタッチされていません");
        }

        characterRB = GetComponent<Rigidbody>();
        if(characterRB == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません");
        }

        groundCheck = GetComponent<GroundCheck>();
        if (groundCheck == null)
        {
            transform.AddComponent<GroundCheck>();
            groundCheck = GetComponent<GroundCheck>();
            Debug.Log("GroundCheckがアタッチされていなかったのでアタッチしました");
        }

        effectController = GetComponent<EffectController>();
        if(effectController == null)
        {
            Debug.LogError("EffectController");
        }

        rendererData = GetComponentInChildren<RendererData>();
        if (rendererData != null)
        {
            rendererData.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("Rendererがアタッチされていません");
        }

        rendererEffect = new RendererEffect(this);

        knockBackCommand = new KnockBackCommand(this);
    }

    protected virtual void  Start()
    {
        //スタート時の初期化
        characterStatus.Velocity = Vector3.zero;
        characterStatus.MoveInput = false;
        battleMode = false;
    }
    protected virtual void Update()
    {
        if (Time.timeScale <= 0) { return; }
        //入力を解除
        characterStatus.MoveInput = false;
        rendererEffect.ColorChange();
    }
    /// <summary>
    /// 入力してるかを判断させるための関数
    /// </summary>
    protected virtual void MoveStateCheck()
    {
        characterStatus.MoveInput = true;
    }
    /// <summary>
    /// オブジェクトを止める関数
    /// </summary>
    public void StopMove()
    {
        characterStatus.Velocity = StopMoveVelocity();
        characterRB.velocity = characterStatus.Velocity;
    }
    public Vector3 StopMoveVelocity()
    {
        return new Vector3(0, characterRB.velocity.y, 0);
    }
    /// <summary>
    /// オブジェクトのRigidbodyに移動量を与える関数
    /// </summary>
    protected void Move()
    {
        characterRB.velocity = new Vector3(characterStatus.Velocity.x, characterRB.velocity.y, characterStatus.Velocity.z);
    }
    /// <summary>
    /// ジャンプのパワーをもらってオブジェクトをy上方向に上げる関数
    /// </summary>
    /// <param name="_jumppower"></param>
    /// <param name="_maxJumpPower"></param>
    public virtual void JumpForce(float _jumppower)
    {
        characterRB.AddForce(transform.up * _jumppower, ForceMode.Impulse);
    }
    /// <summary>
    /// オブジェクトの前方向に引数でもらってきた値分だけ加速させる関数
    /// </summary>
    /// <param name="_accele"></param>
    public virtual void ForwardAccele(float _accele)
    {
        characterRB.AddForce(transform.forward * _accele, ForceMode.Acceleration);
    }
    /// <summary>
    /// キャラクターが死亡した時に呼び出す基底関数
    /// </summary>
    public virtual void Death()
    {
        if (characterStatus.DeathFlag) { return; }
        characterStatus.DeathFlag = true;
    }
    /// <summary>
    /// キャラクターのHPを回復させる関数
    /// maxHPより大きくはならない
    /// </summary>
    /// <param name="count"></param>
    public virtual void RecoveryHelth(int count)
    {
        characterStatus.HP +=count;
        if(characterStatus.HP > characterStatus.GetMaxHP())
        {
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }
}

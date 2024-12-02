using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー、敵の制御クラスの元クラス
/// </summary>
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStatus               characterStatus;
    public CharacterStatus                  CharacterStatus => characterStatus;
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
    [SerializeField]
    protected GroundCheck                   groundCheck;
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
    private bool                            battleMode = false;
    public bool                             BattleMode { get { return battleMode; } set { battleMode = value; } }
    /// <summary>
    /// オブジェクトの表示、非表示を設定する関数3選
    /// </summary>
    /// <returns></returns>
    public GameObject                       SelfObject() { return gameObject; }
    public bool                             IsActiveObject() { return gameObject.activeSelf; }
    public void                             SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }

    /// <summary>
    /// パシフィックマテリアル
    /// </summary>
    [SerializeField]
    protected List<PhysicMaterial> physicMaterials = new List<PhysicMaterial>();
    protected virtual void Awake(){}
    protected virtual void InitializeAssign()
    {
        animator =          GetComponent<Animator>();
        characterCollider = GetComponent<Collider>();
        characterRB =       GetComponent<Rigidbody>();
        effectController =  GetComponent<EffectController>();
        rendererData =      GetComponentInChildren<RendererData>();
        rendererEffect =    new RendererEffect(this);
        knockBackCommand =  new KnockBackCommand(this);

        if(animator == null)
        {
            Debug.LogError("Animatorがアタッチされていません");
        }

        SetMotionController();

        if(characterCollider == null)
        {
            Debug.Log("Colliderがアタッチされていません");
        }

        if(characterRB == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません");
        }

        groundCheck.SetTransform(transform);

        if(effectController == null)
        {
            Debug.LogError("EffectController");
        }

        if (rendererData != null)
        {
            rendererData.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("Rendererがアタッチされていません");
        }
    }

    protected virtual void  Start()
    {
        //スタート時の初期化
        characterStatus.Velocity = Vector3.zero;
        characterStatus.MoveInput = false;
        battleMode = false;
    }
    /// <summary>
    /// PhysicMaterialを着地の有無で変更する関数
    /// </summary>
    protected void SetPhysicMaterial()
    {
        if (characterStatus.CurrentState != CharacterTagList.StateTag.Grab && !characterStatus.Landing)
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Jump];
        }
        else
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Land];
        }
    }
    protected virtual void Update()
    {
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
    public virtual void StopMove()
    {
        characterStatus.Velocity = StopMoveVelocity();
        characterRB.velocity = characterStatus.Velocity;
    }
    public Vector3 StopMoveVelocity()
    {
        Vector3 stopVelocity = new Vector3(0, characterRB.velocity.y, 0);
        if (characterStatus.CurrentState == CharacterTagList.StateTag.Grab||
            characterStatus.CurrentState == CharacterTagList.StateTag.ClimbWall)
        {
            stopVelocity = Vector3.zero;
        }

        return stopVelocity;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGimic : MonoBehaviour
{
    /// <summary>
    /// スイッチがONかOFFか判定するフラグ
    /// </summary>
    [SerializeField]
    private bool                switchFlag = false;
    public bool                 IsSwitchFlag() {  return switchFlag; }
    /// <summary>
    /// スイッチオブジェクトのカラーを変更するクラス
    /// </summary>
    private ColorChange         colorChange = null;
    /// <summary>
    /// スイッチの効果音を管理するクラス
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// スイッチのエフェクトを管理するクラス
    /// </summary>
    private EffectController    effectController = null;
    /// <summary>
    /// エフェクトの大きさを設定する変数
    /// </summary>
    private const float         effectScale = 1.0f;
    /// <summary>
    /// 効果音とエフェクトを発生させるのを止めるタイマークラス
    /// </summary>
    private DeltaTimeCountDown  hitCoolDownTimer = null;
    /// <summary>
    /// タイマークラスに使うカウント変数
    /// </summary>
    private const float         coolDownCount = 0.5f;

    private void Awake()
    {
        colorChange = GetComponentInChildren<ColorChange>();
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }        
        effectController = GetComponent<EffectController>();
        hitCoolDownTimer = new DeltaTimeCountDown();
    }

    private void Update()
    {
        hitCoolDownTimer.Update();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hitCoolDownTimer.IsEnabled()) { return; }
        if(other.tag != "Attack") { return; }
        if (!switchFlag)
        {
            switchFlag = true;
            colorChange.ChangeMaterial(ColorChange.MaterialTag.Two);
        }
        soundController.PlaySESound((int)SoundTagList.SwicthSoundTag.Hit);
        effectController.CreateVFX((int)EffectTagList.SwordHitTag.Hit, other.transform.position,effectScale, Quaternion.identity);
        hitCoolDownTimer.StartTimer(coolDownCount);
    }
}

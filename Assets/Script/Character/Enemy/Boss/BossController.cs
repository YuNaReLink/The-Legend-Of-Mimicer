using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    private BossInput bossStateInput = null;
    public BossInput GetBossStateInput() {  return bossStateInput; }

    [SerializeField]
    private bool stunFlag = false;
    public bool StunFlag { get { return stunFlag; } set { stunFlag = value; } }

    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();

        bossStateInput = new BossInput(this);
        if(bossStateInput == null)
        {
            Debug.LogError("bossStateInputが生成されていません");
        }
        else
        {
            bossStateInput.Initilaize();
        }
    }

    protected override void Update()
    {
        base.Update();
        bossStateInput.StateInput();
        //特定のモーションを特定の条件で止めたり再生したりするメソッド
        motion.StopMotion();
        //特定のモーション終了時に処理を行うメソッド
        motion.EndMotion();
    }

    private void FixedUpdate()
    {
        
    }
}

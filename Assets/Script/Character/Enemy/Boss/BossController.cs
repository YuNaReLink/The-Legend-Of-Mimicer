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
            Debug.LogError("bossStateInput����������Ă��܂���");
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
        //����̃��[�V���������̏����Ŏ~�߂���Đ������肷�郁�\�b�h
        motion.StopMotion();
        //����̃��[�V�����I�����ɏ������s�����\�b�h
        motion.EndMotion();
    }

    private void FixedUpdate()
    {
        
    }
}

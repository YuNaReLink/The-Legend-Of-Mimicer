using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    private BossInput bossStateInput = null;
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
            Debug.LogError("bossStateInputÇ™ê∂ê¨Ç≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }
    }

    protected override void Update()
    {
        bossStateInput.StateInput();
    }

    private void FixedUpdate()
    {
        
    }
}

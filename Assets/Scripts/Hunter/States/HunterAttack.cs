using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAttack : HunterStates
{
    // Referência para o dono do estado
    Hunter hunter;

    public override void Enter(Hunter hunter)
    {
        // Pega a referência para o dono atual do estado
        this.hunter = hunter;
    }

    public override void Update()
    {
        // Chama o método de morte do alvo
        hunter.currentTarget.Death();
        // Muda o estado atual
        hunter.ChangeState(hunter.move);
    }

    public override void Exit()
    {
        
    }
  
}

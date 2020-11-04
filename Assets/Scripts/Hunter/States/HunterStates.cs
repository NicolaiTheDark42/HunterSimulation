using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Molde para os estados do caçador
public abstract class HunterStates
{
    public abstract void Enter(Hunter hunter);
    public abstract void Update();
    public abstract void Exit();
}

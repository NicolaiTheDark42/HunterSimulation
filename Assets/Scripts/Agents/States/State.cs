using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Molde para os estados da caça
public abstract class State
{
    public abstract void Enter(Agent agent);
    public abstract void Update();
    public abstract void Exit();
}

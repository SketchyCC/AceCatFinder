using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatBaseState
{
    public abstract void EnterState(CatFSM Cat);
    public abstract void UpdateState(CatFSM Cat);
    public abstract void CheckState(CatFSM Cat);
}

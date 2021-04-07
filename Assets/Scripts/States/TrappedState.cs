using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrappedState : CatBaseState
{
    //State when the cat is caught by the player, used while you want to check the cat's identity
    
    public override void CheckState(CatFSM Cat)
    {
        //If play gets to select "no" when asked for the cat, this cat should enter fleestate
        if (!Cat.CorrectCat)
        {
            Cat.TransitionState(Cat.fleestate);
        }

    }

    public override void EnterState(CatFSM Cat)
    {
        Cat.navmesh.speed = 0;
    }

    public override void UpdateState(CatFSM Cat)
    {
        
    }
}

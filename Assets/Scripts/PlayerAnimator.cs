using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    public Animator Nanimator;
    private int WalkingHash;
    private int SneakingHash;
    private int BagOpenHash;
    private int NetOutHash;
    private int SwingHash;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        WalkingHash = Animator.StringToHash("IsWalking");
        SneakingHash = Animator.StringToHash("IsSneaking");
        BagOpenHash = Animator.StringToHash("IsBagOpen");
        NetOutHash = Animator.StringToHash("IsNetOut");
        SwingHash = Animator.StringToHash("SwingNow");
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<WalkingEvent>(OnWalkUpdate);
        GameEventManager.AddListener<SneakEvent>(OnSneakUpdate);
        GameEventManager.AddListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.AddListener<NetOutEvent>(OnNetUpdate);
        GameEventManager.AddListener<SwingEvent>(OnSwingUpdate);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<WalkingEvent>(OnWalkUpdate);
        GameEventManager.RemoveListener<SneakEvent>(OnSneakUpdate);
        GameEventManager.RemoveListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.RemoveListener<NetOutEvent>(OnNetUpdate);
        GameEventManager.RemoveListener<SwingEvent>(OnSwingUpdate);
    }
      
    private void OnWalkUpdate(WalkingEvent e)
    {        
        animator.SetBool(WalkingHash, e.IsWalking);
        Nanimator.SetBool(WalkingHash, e.IsWalking);

    }
    private void OnSneakUpdate(SneakEvent e)
    {
        animator.SetBool(SneakingHash, e.IsSneaking);
        Nanimator.SetBool(SneakingHash, e.IsSneaking);
    }
       
    private void OnBagUpdate(BagOpenEvent e)
    {
        animator.SetBool(BagOpenHash, e.openBag);
        Nanimator.SetBool(BagOpenHash, e.openBag);
    }
    private void OnNetUpdate(NetOutEvent e)
    {
        animator.SetBool(NetOutHash, e.netOut);
        Nanimator.SetBool(NetOutHash, e.netOut);
    }
    private void OnSwingUpdate(SwingEvent e)
    {
        animator.SetTrigger(SwingHash);  
        Nanimator.SetTrigger(SwingHash);
    }
}

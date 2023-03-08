using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, IPlayerAnimationHandler
{
    private const string WALKING = "Movement";
    private const string IDLE = "Idle";
    private const string MINE = "Mine";

    private Animator _animator;

    private bool _isAlreadyWalking;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetWalking(bool value)
    {
        Debug.Log("Movement");
        _animator.SetBool(WALKING, value);
    }

    
    public void SetMine()
    {
        _animator.SetTrigger(MINE);
    }
}

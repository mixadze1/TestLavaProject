using Assets._Scripts.Interfaces;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, IPlayerAnimationHandler
{
    private const string WALKING = "Movement";
    private const string MINE = "Mine";

    private Animator _animator;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetWalking(bool value)
    {
        _animator.SetBool(WALKING, value);
    }

    
    public void SetMine(bool value)
    {
        _animator.SetBool(MINE, value);
    }
}

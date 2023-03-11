using Assets._Scripts.Game;
using Assets._Scripts.Interfaces;
using UnityEngine;

public class ResourceDigger : MonoBehaviour, IFixedUpdater
{
    [SerializeField, Range(1f,5f)] private float _range = 8f;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _damage = 15;

    private IMovementHandler _playerMovementHandler;
    private IPlayerAnimationHandler _animationHandler;

    private ResourceSource _resource;

    private float _fixedTime;
    private float _timeDamage;

    private bool _isTouch;

    public void Initialize(IMovementHandler movementHandler, IPlayerAnimationHandler animationHandler)
    {
        _playerMovementHandler = movementHandler;
        _animationHandler = animationHandler;
    }

    public bool IsTouch()
    {
        return _isTouch;
    }

    private void Digger()
    {
        _resource.GetDamage(_damage);
    }

    public void FixedUpdater()
    {
        if (CheckResourceInRange() && _playerMovementHandler.IsMovement() &&  _resource.GetHealth() > 0)
        {

            _resource.EnableView();
            _animationHandler.SetMine(true);
            _fixedTime += Time.fixedDeltaTime;
            if (_fixedTime >= _timeDamage)
            {
                _isTouch = true;
                Debug.Log("Digger");
                Digger();
                _fixedTime = 0;
            }           
        }
        if(!CheckResourceInRange() || _resource.GetHealth() <= 0)
        {
            _resource?.RecoveryEnable();
            _resource = null;
            _animationHandler.SetMine(false);
        }
    }

    private bool CheckResourceInRange()
    {
        var position = transform.position;
        RaycastHit hitCollider;

        Ray rayBackward = new Ray(new Vector3(position.x, position.y, position.z), transform.forward);
        Debug.DrawRay(new Vector3(position.x, position.y, position.z), transform.forward * _range);

        if (Physics.Raycast(rayBackward, out hitCollider, _range, _layerMask))
        {
            _resource = hitCollider.transform.GetComponent<ResourceSource>();
            _timeDamage = _resource.GetTimeDamage();
            return true;

        }
        return false;
    }
}

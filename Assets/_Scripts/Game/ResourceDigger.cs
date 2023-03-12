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

    private IResourceSourceHandler _resourceHadnler;

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
        _resourceHadnler.GetDamage(_damage);
    }

    public void FixedUpdater()
    {
        if (CheckResourceInRange() && _playerMovementHandler.IsMovement() &&  _resourceHadnler.GetHealth() > 0)
        {
            _resourceHadnler.EnableView();
            _animationHandler.SetMine(true);
            _fixedTime += Time.fixedDeltaTime;
            if (_fixedTime >= _timeDamage)
            {
                Debug.Log("Digger");

                _isTouch = true;
                Digger();
                _fixedTime = 0;
            }
        }

        if (_resourceHadnler?.GetHealth() <= 0 || !CheckResourceInRange())
            _animationHandler.SetMine(false);

        if (!CheckResourceInRange() && _resourceHadnler != null)
        {     
            if (!_resourceHadnler.IsDelayRecovery() && _resourceHadnler.GetHealth() < _resourceHadnler.GetMaxHealth() && !_resourceHadnler.IsRecovery())
            {
                Debug.Log("Recovery");
                _resourceHadnler?.RecoveryEnable();
            }
        }
    }

    private bool CheckResourceInRange()
    {
        var position = transform.position;
        RaycastHit hitCollider;

        var offset = 0.5f;
        Ray rayBackward = new Ray(new Vector3(position.x, position.y + offset, position.z), transform.forward);
        Debug.DrawRay(new Vector3(position.x, position.y + offset, position.z), transform.forward * _range, Color.red);

        if (Physics.Raycast(rayBackward, out hitCollider, _range, _layerMask))
        {
            _resourceHadnler = hitCollider.transform.GetComponent<IResourceSourceHandler>();
            _timeDamage = _resourceHadnler.GetTimeDamage();
            return true;
        }
        return false;
    }
}

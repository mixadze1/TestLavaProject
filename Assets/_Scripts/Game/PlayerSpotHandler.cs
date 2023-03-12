using Assets._Scripts.Interfaces;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class PlayerSpotHandler : MonoBehaviour, IFixedUpdater
    {
        [SerializeField] private LayerMask _layerMask;

        private IJoystickHandler _joystickHandler;
        private IResourceHandler _resourceHandler;

        private Spot _spot;

        private float _offsetY = 1;
        private float _range = 3;

        private bool _isTouch;

        public void Initialize(IJoystickHandler joystickHandler, IResourceHandler resourceHandler)
        {
            _resourceHandler = resourceHandler;
            _joystickHandler = joystickHandler;
        }

        public bool IsTouch()
        {
            return _isTouch;
        }

        public void FixedUpdater()
        {
            if(CheckSpotUnderPlayer() && !_joystickHandler.IsJoystickEnable()) 
            {
               var typeResource =  _spot.TypeResource();
                if(_resourceHandler.AmountResource(typeResource) > 0)
                {
                    _isTouch = true;
                    GiveSpotResource(typeResource);
                }
            }
        }

        private void GiveSpotResource(ResourceType type)
        {
            var amountGive = 1;
           var completeOperation = _spot.GetNeedResource(type, this.transform.position, amountGive);
            if (completeOperation)
            {
                Debug.Log("GiveResource " + type); 
                _resourceHandler.RemoveResource(type, amountGive);
            }
     
        }

        private bool CheckSpotUnderPlayer()
        {
            var position = transform.position;
            RaycastHit hitCollider;

            Ray rayBackward = new Ray(new Vector3(position.x, position.y + _offsetY, position.z), -transform.up);
            Debug.DrawRay(new Vector3(position.x, position.y, position.z), -transform.up * _range);

            if (Physics.Raycast(rayBackward, out hitCollider, _range, _layerMask))
            {
                _spot = hitCollider.transform.GetComponent<Spot>();
                return true;
            }
            return false;
        }
    }
}
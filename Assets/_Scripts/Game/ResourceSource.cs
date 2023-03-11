using Assets._Scripts.Interfaces;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Game
{
    public class ResourceSource : MonoBehaviour, IFixedUpdater
    {
    
        [SerializeField] private Image[] _imageSlider;
        [SerializeField] private Slider _slider;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Resource _itemPrefab;


        private ResourceContainer _resourceContainer;
        private Coroutine _coroutine;
        private Transform _cameraTransform;

        private Material _material;

        private float _recoveryHealthPerFixedUpdate;
        private float _damageScaler;
        private float _timeDamage;
        private float _rangeResource;

        private float _jumpPower;
        private float _timeMoveResource;

        private int _countResource;

        private bool _isRecovery;

        private WaitForSeconds _delay;

        public float Health { get; private set; }
        public float MaxHealth { get; private set; }

        public ResourceType ResourceType { get; private set; }

        public void Initialize(ResourceFactory.ResourceConfig config, Position position, Transform camera, ResourceContainer resourceContainer)
        {
            SetPosition(position);
            _resourceContainer = resourceContainer;
           var meshRenderer = GetComponent<MeshRenderer>();
            _cameraTransform = camera;
            _material = config.Material;
            _rangeResource = config.RangeCreateResource;
            meshRenderer.material = _material;
            Health = config.Health;
            _delay = new WaitForSeconds(config.DelayRecovery);

            _jumpPower = config.ResourceJumpPower;
            _timeMoveResource = config.ResourceTimeMove;

            MaxHealth = config.Health;
            _countResource = config.AmountCreateResource;
            _recoveryHealthPerFixedUpdate = config.RecoveryHealthPerSecond * Time.fixedDeltaTime;
            _damageScaler = config.ScalerDamage;
            ResourceType = config.Type;
            _timeDamage = config.TimeDamage;
            ColorSlider();
            SLiderView(Health);
        }

        public float GetTimeDamage()
        {
            return _timeDamage;
        }

        public void EnableView()
        {
            if(!_slider.gameObject.activeSelf)
                _slider.gameObject.SetActive(true);
        }

        public void DisableView()
        {
            _slider.gameObject.SetActive(false);
        }

        public void RecoveryEnable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
           _coroutine = StartCoroutine(DelayRecovery());
        }

        private IEnumerator DelayRecovery()
        {
            yield return _delay;
            _isRecovery = true;
        }

        private bool IsRecovery()
        {
            if(_isRecovery)
                return true;
            return false;
        }

        public void FixedUpdater()
        {
            var transform = _cameraTransform;
            _canvas.transform.LookAt(transform);

            if(IsRecovery() && Health / MaxHealth < 1)
            {
                Recovery();
            }
        }

        private void Recovery()
        {
            EnableView();
            Health += _recoveryHealthPerFixedUpdate;

            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
                _isRecovery = false;
                DisableView();
            }
            SLiderView(Health);
        }

        private void ColorSlider()
        {
            foreach(var image in _imageSlider)
            {
                image.color = _material.color;
            }
        }

        private void SLiderView(float health)
        {
            _slider.value = health / MaxHealth;
            if(_slider.value <= 0)
                DisableView();
        }

        private void SetPosition(Position position)
        {
            Vector3 myPosition = position.transform.position;
            var offset = 1.5f;
            this.transform.position = new Vector3(myPosition.x, offset, myPosition.z);
        }

        public float GetHealth()
        {
            return Health;
        }

        public void GetDamage(float damage)
        {
            Debug.Log(Health);
            if(Health > 0)
            {
                _isRecovery = false;
                var calculateDamage = damage * _damageScaler;
                Debug.Log("damage");
                this.transform.DOScale(0.85f, 0.3f).OnComplete(() => this.transform.DOScale(1f, 0.3f));
                Health -= calculateDamage;
                SLiderView(Health);
                GetItem();
            }
        }

        private void GetItem()
        {
            for(int i = 0; i < _countResource; i++)
            {
                var item = Instantiate(_itemPrefab, this.transform.position, Quaternion.identity);
                item.transform.SetParent(_resourceContainer.transform);
                item.Initialize(ResourceType);
                MoveResource(item);
            }
        }

        private void MoveResource(Resource item)
        {
            int amountJump = 1;
            var direction = new Vector3(this.transform.position.x + Random.Range(-_rangeResource, _rangeResource), this.transform.position.y, this.transform.position.z + Random.Range(-_rangeResource, _rangeResource));
            item.transform.DOJump(new Vector3(direction.x, 1.15f, direction.z), _jumpPower, amountJump, _timeMoveResource).OnComplete(() => item.GetBoxCollider().enabled = true);
        }
    }
}
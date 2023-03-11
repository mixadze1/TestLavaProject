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

        private ParticleSystem _particle;

        private ResourceContainer _resourceContainer;
        private Coroutine _coroutine;
        private Transform _cameraTransform;

        private Material _material;

        private float _recoveryHealthPerFixedUpdate;
        private float _damageScaler;
        private float _timeDamage;
        private float _rangeResource;

        private float _jumpPowerResource;
        private float _timeMoveResource;

        private int _countResource;

        private bool _isRecovery;

        private WaitForSeconds _delay;

        public float Health { get; private set; }
        public float MaxHealth { get; private set; }

        public ResourceType ResourceType { get; private set; }

        public void Initialize(ResourceFactory.ResourceConfig config, Position position, Transform camera, ResourceContainer resourceContainer)
        {
            _resourceContainer = resourceContainer;

            SetPosition(position);      
            InitializeColor(config);
            InitializeCameraTransform(camera);
            var health = InitializeResourceSourceParametr(config);
            InitializeParticle(config);
            InitializeResourceParametr(config);
            ColorSlider();
            SLiderView(health);
        }

        private void InitializeResourceParametr(ResourceFactory.ResourceConfig config)
        {
            _jumpPowerResource = config.ResourceJumpPower;
            _timeMoveResource = config.ResourceTimeMove;
            _countResource = config.AmountCreateResource;
            _rangeResource = config.RangeCreateResource;
        }

        private float InitializeResourceSourceParametr(ResourceFactory.ResourceConfig config)
        {
            Health = config.Health;
            MaxHealth = config.Health;
            _recoveryHealthPerFixedUpdate = config.RecoveryHealthPerSecond * Time.fixedDeltaTime;
            _damageScaler = config.ScalerDamage;
            ResourceType = config.Type;
            _timeDamage = config.TimeDamage;
            _delay = new WaitForSeconds(config.DelayRecovery);
            return Health;
        }

        private void InitializeParticle(ResourceFactory.ResourceConfig config)
        {
            var offset = 1;
            _particle = Instantiate(config.Particle, new Vector3(this.transform.position.x, this.transform.position.y + offset, this.transform.position.z), Quaternion.identity);
            _particle.transform.SetParent(this.transform);
        }

        private void InitializeCameraTransform(Transform camera)
        {
            _cameraTransform = camera;
        }

        private void InitializeColor(ResourceFactory.ResourceConfig config)
        {
            _material = config.Material;
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = _material;
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
            if(Health > 0)
            {
                _particle.Play();
                _isRecovery = false;
                var calculateDamage = damage * _damageScaler;
                float reduceCube = 0.85f, timeTween = 0.3f;
                this.transform.DOScale(reduceCube, timeTween).OnComplete(() => this.transform.DOScale(1f, timeTween));
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
            float positionFloorY = 1.15f;

            var direction = new Vector3(this.transform.position.x + Random.Range(-_rangeResource, _rangeResource), this.transform.position.y, 
                this.transform.position.z + Random.Range(-_rangeResource, _rangeResource));      

            item.transform.DOJump(new Vector3(direction.x, positionFloorY, direction.z), _jumpPowerResource, amountJump, _timeMoveResource).
                OnComplete(() => item.BoxColliderEnabled(true));
        }
    }
}
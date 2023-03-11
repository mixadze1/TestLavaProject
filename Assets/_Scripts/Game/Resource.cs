using Assets._Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private BoxCollider _boxCollider;

        private float _jumpPower = 3f;
        private float _timeJump = 1.5f;

        private int _amountJump = 1;
        public ResourceType Type { get; private set; }

        public void Initialize(ResourceType type)
        {
            Type = type;
            InitializeMaterial();
        }

        public void GiveItem(IPlayerHandler playerHandler)
        {
            _boxCollider.enabled = false;
            this.transform.SetParent(playerHandler.GetTransform());
            this.transform.rotation = Quaternion.identity;
            this.transform.DOLocalJump(playerHandler.GetPositionForItem().localPosition, _jumpPower, _amountJump, _timeJump).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }

        public Color GetColor() => _meshRenderer.material.color;

        public void BoxColliderEnabled(bool value)
        {
            _boxCollider.enabled = value;
        }

        public MeshRenderer GetMeshRenderer() => _meshRenderer;

        private void InitializeMaterial()
        {
            switch (Type)
            {
                case ResourceType.Trees:
                    _meshRenderer.material = _materials[0];
                    break;
                case ResourceType.Metal:
                    _meshRenderer.material = _materials[1];
                    break;
                case ResourceType.Crystal:
                    _meshRenderer.material = _materials[2];
                    break;
            }
        }
    }
}
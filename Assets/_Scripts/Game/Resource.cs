﻿using Assets._Scripts.Interfaces;
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
        private float _timeRotate = 1.7f;

        private int _amountJump = 1;
        public ResourceType Type {get; private set;}

        public void Initialize(ResourceType type)
        {
            Type = type;
            GetMaterial();
        }

        public void GiveItem(IPlayerHandler playerHandler)
        {
            _boxCollider.enabled = false;
            this.transform.SetParent(playerHandler.GetTransform());
            Debug.Log(playerHandler.GetTransform());
            this.transform.DOLocalJump(playerHandler.GetPositionForItem().localPosition, _jumpPower, _amountJump, _timeJump);
            this.transform.DORotate(Vector3.zero, _timeRotate).OnComplete(() =>
            { 
                Destroy(this.gameObject);
            });
        }

        public BoxCollider GetBoxCollider() => _boxCollider;    
        public MeshRenderer GetMeshRenderer() => _meshRenderer;

        private void GetMaterial()
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
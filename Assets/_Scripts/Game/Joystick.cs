using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets._Scripts.Game
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField] private GameObject touchMarker;
        [SerializeField] private float joystickRadiusInPixels = 150f;
        private GraphicRaycaster graphicRaycaster;
        private Image touchMarkerRenderer;
        private Image joystickRenderer;
        private Vector3 touchOffset;
        private Vector3 initialInput;
        private Vector3 input;
        private bool _isPause;

        public event Action TapOnHospital;
        public event Action TapOnStore;
        public event Action TapOnGunShop;

        public bool IsActiveJoystick;

        private void Awake()
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            touchMarkerRenderer = touchMarker.GetComponent<Image>();
            joystickRenderer = gameObject.GetComponent<Image>();
            touchMarker.transform.position = transform.position;
            HideJoystick();
        }

        private void Update()
        {
            if (_isPause)
            {
                HideJoystick();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                initialInput = Input.mousePosition;
                transform.position = initialInput;
                IsActiveJoystick = true;
                ShowJoystick();
            }

            if (Input.GetMouseButton(0) && (initialInput - transform.position).sqrMagnitude < joystickRadiusInPixels * joystickRadiusInPixels)
            {
                Vector3 touchPosition = Input.mousePosition;
                touchOffset = touchPosition - transform.position;
                input = Vector3.ClampMagnitude(touchOffset, joystickRadiusInPixels);
                touchMarker.transform.position = transform.position + input;
            }
            else
            {
                touchMarker.transform.position = transform.position;
                input = Vector3.zero;
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsActiveJoystick = false;
                HideJoystick();
            }
        }

        public Vector3 GetHorizontalInput()
        {
            Vector3 result = input / joystickRadiusInPixels;
            result = new Vector3(result.x, 0, result.y);
            return result;
        }

        private void HideJoystick()
        {
            ToggleJoystickVisibility(false);
        }

        private void ShowJoystick()
        {
            ToggleJoystickVisibility(true);
        }

        private void ToggleJoystickVisibility(bool state)
        {
            joystickRenderer.enabled = state;
            touchMarkerRenderer.enabled = state;
        }

        public void DisableJoystick()
        {
            input = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void EnableJoystick()
        {
            input = Vector3.zero;
            touchMarker.SetActive(true);
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            HideJoystick();
            Time.timeScale = 1;
        }

        private void OnDisable()
        {
            Time.timeScale = 0;
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void UnPause()
        {
            _isPause = false;
        }

        public void PauseWithInteractableJoystick()
        {
            _isPause = false;
        }
    }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace My_Assets.BarScene.Controllers
{
    public class QuicktimeEventController : MonoBehaviour
    {
        public SitSpot sitSpot;

        public GameObject mouseClickImage;
        public GameObject zoneRotator;
        public GameObject bottleRotator;
        public TextMeshProUGUI messageTMP;

        public float baseSpeed = 60.0f;

        private float _rotationSpeed;
        private float _currentBottleRotation;

        private int _bottleStartRotation;
        private int _zoneRotation;

        private bool _isFinished;
        private bool _isSuccessful;

        private string _successMessage = "<color=#008000>Well done!</color> \nYou're handling it well my friend.";
        private string _failMessage = "<color=#FF0000>Oof!</color> \nIt's tough isn't it.";

        private void Update()
        {
            if (!_isFinished)
            {
                float rotation = _rotationSpeed * -1 * Time.deltaTime;
                bottleRotator.transform.Rotate(0, 0, rotation, Space.Self);
                _currentBottleRotation += rotation;
            
            
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    _isSuccessful = _currentBottleRotation % 360 > _zoneRotation - 30 &&
                                         _currentBottleRotation % 360 < _zoneRotation + 30;

                    _isFinished = true;

                    StartCoroutine(ShowSuccess());
                }
            }
        }

        public void Init()
        {
            _isFinished = false;
            _isSuccessful = false;
            messageTMP.text = "";
            mouseClickImage.SetActive(true);
            _bottleStartRotation = Random.Range(0, 359);
            _zoneRotation = Random.Range(_bottleStartRotation + 90, _bottleStartRotation + 320) % 360;
            _bottleStartRotation *= -1;
            _zoneRotation *= -1;
            _currentBottleRotation = _bottleStartRotation;
            
            bottleRotator.transform.Rotate(0, 0, _bottleStartRotation);
            zoneRotator.transform.Rotate(0, 0, _zoneRotation);
            
            _rotationSpeed = baseSpeed * PlayerPrefs.GetInt("Level") * (PlayerPrefs.GetInt("Drunkness") + 1);
            gameObject.SetActive(true);
        }

        /**
         * TODO: Story achtig iets toevoegen. Doe dit vóór de yield return om het na het finishen van de QTE te doen.
         * Je kan evt ook nog een textbox boven de QTE zetten met 'beer O'clock' oid. Kijk maar wat je leuk lijkt.
         */
        private IEnumerator ShowSuccess()
        {
            mouseClickImage.SetActive(false);
            
            messageTMP.text = _isSuccessful ? _successMessage : _failMessage;
            
            yield return new WaitForSeconds(1.5f);
            
            bottleRotator.transform.localEulerAngles = Vector3.zero;
            zoneRotator.transform.localEulerAngles = Vector3.zero;
            
            sitSpot.DrinkingQTEResult(_isSuccessful);
            gameObject.SetActive(false);
        }
    }
}
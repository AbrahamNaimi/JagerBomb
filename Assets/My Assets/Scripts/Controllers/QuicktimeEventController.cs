using System;
using System.Collections;
using My_Assets.Bar_Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace My_Assets.Controllers
{
    public class QuicktimeEventController : MonoBehaviour
    {
        public SitSpot sitSpot;
        
        public GameObject zoneRotator;
        public GameObject bottleRotator;

        private float rotationSpeed = 180.0f;
        private float _currentBottleRotation;

        private int _bottleStartRotation;
        private int _zoneRotation;

        public void Start()
        {
            _bottleStartRotation = Random.Range(0, 359);
            _zoneRotation = Random.Range(_bottleStartRotation + 90, _bottleStartRotation + 320) % 360;
            _bottleStartRotation *= -1;
            _zoneRotation *= -1;
            _currentBottleRotation = _bottleStartRotation;
            
            bottleRotator.transform.Rotate(0, 0, _bottleStartRotation);
            zoneRotator.transform.Rotate(0, 0, _zoneRotation);
            
            rotationSpeed = 60.0f * PlayerPrefs.GetInt("Level") * (PlayerPrefs.GetInt("Drunkness") + 1);
        }

        private void Update()
        {
            float rotation = rotationSpeed * -1 * Time.deltaTime;
            bottleRotator.transform.Rotate(0, 0, rotation, Space.Self);
            _currentBottleRotation += rotation;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                bool success = _currentBottleRotation % 360 > _zoneRotation - 30 &&
                               _currentBottleRotation % 360 < _zoneRotation + 30;
                sitSpot.DrinkingQTEFinished(success);
                Destroy(gameObject);
            }
        }

        // public GameObject QteObject;
        // public SitSpot sitSpot;
        // public TextMeshProUGUI DisplayBox;
        // public TextMeshProUGUI PassBox;
        //
        // public int QTEGen;
        // public int WaitingForKey;
        // public bool CorrectKeyPressed;
        // public bool CountingDown;
        //
        //
        // public void Update()
        // {
        //     if (WaitingForKey == 0)
        //     {
        //         QTEGen = Random.Range(0, 3);
        //         CountingDown = true;
        //         StartCoroutine(CountDown());
        //
        //         switch (QTEGen)
        //         {
        //             case 0:
        //                 WaitingForKey = 1;
        //                 DisplayBox.text = "Q";
        //                 break;
        //
        //             case 1:
        //                 WaitingForKey = 1;
        //                 DisplayBox.text = "R";
        //                 break;
        //
        //             case 2:
        //                 WaitingForKey = 1;
        //                 DisplayBox.text = "T";
        //                 break;
        //         }
        //     }
        //
        //
        //     if (WaitingForKey == 1)
        //     {
        //         if (Keyboard.current.anyKey.wasPressedThisFrame)
        //         {
        //             switch (QTEGen)
        //             {
        //                 case 0:
        //                     CorrectKeyPressed = Keyboard.current.qKey.wasPressedThisFrame;
        //                     break;
        //
        //                 case 1:
        //                     CorrectKeyPressed = Keyboard.current.rKey.wasPressedThisFrame;
        //                     break;
        //
        //                 case 2:
        //                     CorrectKeyPressed = Keyboard.current.tKey.wasPressedThisFrame;
        //                     break;
        //             }
        //             StartCoroutine(KeyPressing());
        //             WaitingForKey = -1;
        //         }
        //     }
        // }
        //
        // IEnumerator KeyPressing()
        // {
        //     QTEGen = 10;
        //     if (!CorrectKeyPressed)
        //     {
        //         StartCoroutine(sitSpot.SitAndDrink());
        //         PassBox.text = "Incorrect. Man that bums me out, I can't do anything. Time for a drink.";
        //     }
        //     else
        //     {
        //         PassBox.text = "I gotta lay off the booze man";
        //     }
        //     CountingDown = false;
        //     yield return new WaitForSeconds(1.5f);
        //     Destroy(QteObject);
        // }
        //
        // IEnumerator CountDown()
        // {
        //     yield return new WaitForSeconds(1.5f);
        //     if (CountingDown)
        //     {
        //         StartCoroutine(sitSpot.SitAndDrink());
        //         PassBox.text = "Too late. Man that bums me out, I can't do anything right. Time for a drink.";
        //     }
        //
        //     yield return new WaitForSeconds(1.5f);
        //     Destroy(QteObject);
        // }
    }
}
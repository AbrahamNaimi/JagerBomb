using System.Collections;
using My_Assets.Bar_Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace My_Assets.Controllers
{
    public class QuicktimeEventController : MonoBehaviour
    {
        public GameObject QteObject;
        public SitSpot sitSpot;
        public TextMeshProUGUI DisplayBox;
        public TextMeshProUGUI PassBox;

        public int QTEGen;
        public int WaitingForKey;
        public bool CorrectKeyPressed;
        public bool CountingDown;


        public void Update()
        {
            if (WaitingForKey == 0)
            {
                QTEGen = Random.Range(0, 3);
                CountingDown = true;
                StartCoroutine(CountDown());

                switch (QTEGen)
                {
                    case 0:
                        WaitingForKey = 1;
                        DisplayBox.text = "Q";
                        break;

                    case 1:
                        WaitingForKey = 1;
                        DisplayBox.text = "R";
                        break;

                    case 2:
                        WaitingForKey = 1;
                        DisplayBox.text = "T";
                        break;
                }
            }


            if (WaitingForKey == 1)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    switch (QTEGen)
                    {
                        case 0:
                            CorrectKeyPressed = Keyboard.current.qKey.wasPressedThisFrame;
                            break;

                        case 1:
                            CorrectKeyPressed = Keyboard.current.rKey.wasPressedThisFrame;
                            break;

                        case 2:
                            CorrectKeyPressed = Keyboard.current.tKey.wasPressedThisFrame;
                            break;
                    }
                    StartCoroutine(KeyPressing());
                    WaitingForKey = -1;
                }
            }
        }

        IEnumerator KeyPressing()
        {
            QTEGen = 10;
            if (!CorrectKeyPressed)
            {
                StartCoroutine(sitSpot.SitAndDrink());
                PassBox.text = "Incorrect. Man that bums me out, I can't do anything. Time for a drink.";
            }
            else
            {
                PassBox.text = "I gotta lay off the booze man";
            }
            CountingDown = false;
            yield return new WaitForSeconds(1.5f);
            Destroy(QteObject);
        }

        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(1.5f);
            if (CountingDown)
            {
                StartCoroutine(sitSpot.SitAndDrink());
                PassBox.text = "Too late. Man that bums me out, I can't do anything right. Time for a drink.";
            }
        
            yield return new WaitForSeconds(1.5f);
            Destroy(QteObject);
        }
    }
}
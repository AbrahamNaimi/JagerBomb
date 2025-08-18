using System.Collections;
using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem; 

public class QTESys : MonoBehaviour
{
    public TextMeshProUGUI DisplayBox; 
    public TextMeshProUGUI PassBox;   

    public int QTEGen;
    public int WaitingForKey;
    public int CorrectKey;
    public int CountingDown;

    public void Update()
    {
        if (WaitingForKey == 0)
        {
            QTEGen = Random.Range(0, 3);
            CountingDown = 1;
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
                        CorrectKey = Keyboard.current.qKey.wasPressedThisFrame ? 1 : 2;
                        StartCoroutine(KeyPressing());
                        break;

                    case 1: 
                        CorrectKey = Keyboard.current.rKey.wasPressedThisFrame ? 1 : 2;
                        StartCoroutine(KeyPressing());
                        break;

                    case 2: 
                        CorrectKey = Keyboard.current.tKey.wasPressedThisFrame ? 1 : 2;
                        StartCoroutine(KeyPressing());
                        break;
                }
            }
        }
    }

    IEnumerator KeyPressing()
    {
        QTEGen = 10;
        if (CorrectKey == 1)
        {
            CountingDown = 2;
            PassBox.text = "PASS!";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = 0;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }

        if (CorrectKey == 2)
        {
            CountingDown = 2;
            PassBox.text = "FAIL!!!!";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = 0;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1.5f);
        if (CountingDown == 1)
        {
            QTEGen = 10;
            CountingDown = 2;
            PassBox.text = "FAIL!!!!";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = 0;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }
    }
}

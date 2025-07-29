using System.Collections;
using UnityEngine;

public class DaySceneController : MonoBehaviour
{

    // Simulate OnPlayerDoneDrinking call
    // Temporary code
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        OnPlayerFinishDay();
    }

    public void OnPlayerFinishDay()
    {
        GameSceneManager.Instance.GoToNextLevel();
    }

    public void OnPlayerFailedDay()
    {
        GameSceneManager.Instance.Explode();
    } 

}

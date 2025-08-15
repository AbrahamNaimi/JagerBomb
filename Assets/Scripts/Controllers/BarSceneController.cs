using System.Collections;
using UnityEngine;

public class BarSceneController : MonoBehaviour
{

    // Simulate OnPlayerDoneDrinking call
    // Temporary code
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        OnPlayerDoneDrinking();
    }

    public void OnPlayerDoneDrinking()
    {
        GameSceneManager.Instance.LoadCurrentLevelScene();
    }

}

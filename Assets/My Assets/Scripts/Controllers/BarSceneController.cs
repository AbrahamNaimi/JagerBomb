using System.Collections;
using My_Assets.Managers;
using UnityEngine;

namespace My_Assets.Controllers
{
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
}

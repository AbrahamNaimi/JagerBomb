using System.Collections;
using My_Assets.Managers;
using UnityEngine;

namespace My_Assets.Controllers
{
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
}

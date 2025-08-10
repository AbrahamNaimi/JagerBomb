using UnityEngine;
using System.Collections;

public class Drinkable : MonoBehaviour
{
    [Header("Timings")]
    public float moveDuration = 1.0f;      // time to move to mouth
    public float sipDuration  = 1.0f;      // time to "drink" (pause at mouth)
    public float returnDuration = 1.0f;    // time to return

    [Header("Target")]
    public Transform seatAnchor;           // optional: assign SitSpot.seatAnchor; if null, uses camera

    public bool IsBusy { get; private set; }

    public void Drink(Transform cam)
    {
        if (IsBusy) return;
        StartCoroutine(DrinkRoutine(cam));
    }

    private IEnumerator DrinkRoutine(Transform cam)
    {
        IsBusy = true;

        // capture start
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // pick target (prefer seatAnchor)
        Vector3 targetPos = seatAnchor ? seatAnchor.position : cam.position;
        Quaternion targetRot = seatAnchor ? seatAnchor.rotation : cam.rotation;

        // move to target
        yield return LerpPose(startPos, targetPos, startRot, targetRot, moveDuration);

        // sip (just wait at mouth)
        if (sipDuration > 0f)
            yield return new WaitForSeconds(sipDuration);

        // return to start
        yield return LerpPose(targetPos, startPos, targetRot, startRot, returnDuration);

        IsBusy = false;
    }

    private IEnumerator LerpPose(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot, float duration)
    {
        duration = Mathf.Max(0.0001f, duration);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.LerpUnclamped(fromPos, toPos, s);
            transform.rotation = Quaternion.SlerpUnclamped(fromRot, toRot, s);
            yield return null;
        }
        transform.position = toPos;
        transform.rotation = toRot;
    }
}

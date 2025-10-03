using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float UpDistance = 1.0f;
    public float upDuration = 0.2f;       // fast up
    public float downDuration = 0.6f;     // slower down
    public float holdUpTime = 0.3f;       // time spike stays up
    public float holdDownTime = 0.5f;     // time spike stays down
    public float idleTime = 0.5f;         // extra idle before repeating
    public bool startWhenAwake = true;
    public bool useLocalPosition = true;

    Vector2 startPos;
    Vector2 upPos;
    Coroutine loopCoroutine;

    void Awake()
    {
        Vector3 initial = useLocalPosition ? transform.localPosition : transform.position;

        startPos = new Vector2(initial.x, initial.y);
        upPos = startPos + (Vector2.up * UpDistance);

        if (startWhenAwake)
            loopCoroutine = StartCoroutine(PulseLoop());
    }

    public void StartPulse()
    {
        if (loopCoroutine == null)
            loopCoroutine = StartCoroutine(PulseLoop());
    }

    public void StopPulse()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }

    IEnumerator PulseLoop()
    {
        while (true)
        {
            // Spike goes up quickly
            yield return StartCoroutine(MoveSpike(startPos, upPos, upDuration, EaseOutQuad));
            yield return new WaitForSeconds(holdUpTime);

            // Spike goes down slowly
            yield return StartCoroutine(MoveSpike(upPos, startPos, downDuration, EaseInQuad));
            yield return new WaitForSeconds(holdDownTime);

            // Optional extra idle before repeating
            yield return new WaitForSeconds(idleTime);
        }
    }

    IEnumerator MoveSpike(Vector2 fromPos, Vector2 toPos, float duration, System.Func<float, float> ease)
    {
        if (duration <= 0f)
        {
            SetSpikePosition(toPos);
            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float easedT = Mathf.Clamp01(ease(t));
            Vector2 newPos = Vector2.Lerp(fromPos, toPos, easedT);
            SetSpikePosition(newPos);
            yield return null;
        }
        SetSpikePosition(toPos);
    }

    void SetSpikePosition(Vector2 newPos)
    {
        if (useLocalPosition)
        {
            Vector3 current = transform.localPosition;
            transform.localPosition = new Vector3(newPos.x, newPos.y, current.z);
        }
        else
        {
            Vector3 current = transform.position;
            transform.position = new Vector3(newPos.x, newPos.y, current.z);
        }
    }

    float EaseOutQuad(float x) => 1 - (1 - x) * (1 - x);
    float EaseInQuad(float x) => x * x;
}

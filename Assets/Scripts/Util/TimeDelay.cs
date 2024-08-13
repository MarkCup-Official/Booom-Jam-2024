using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDelay : BaseMonoManager<TimeDelay>
{
    public void Delay(float delayTime, System.Action action)
    {
        StartCoroutine(DelayCoroutine(delayTime, action));
    }
    private IEnumerator DelayCoroutine(float delayTime, System.Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}

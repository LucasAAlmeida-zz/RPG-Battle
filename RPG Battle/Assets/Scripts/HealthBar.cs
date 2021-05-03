using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform bar;

    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetHealthPercent(float healthPercentage)
    {
        StartCoroutine(DecreaseHealthRoutine(healthPercentage));
    }

    IEnumerator DecreaseHealthRoutine(float healthPercentage)
    {
        while (healthPercentage < bar.localScale.x) {
            bar.localScale = new Vector3(bar.localScale.x - Time.deltaTime, 1f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}

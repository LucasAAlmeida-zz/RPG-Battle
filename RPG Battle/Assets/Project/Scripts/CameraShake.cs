using System.Collections;
using UnityEngine;

public class CameraShake
{
    public static IEnumerator Shake(float duration, float magnitude)
    {
        var originalPosition = Camera.main.transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration) {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPosition;
    }
}

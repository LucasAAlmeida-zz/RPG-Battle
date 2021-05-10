using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static void Create(Vector3 position, string damage, bool isCritical = true)
    {
        var damagePopupTransform = Instantiate(AssetManager.i.damagePopup, position, Quaternion.identity);
        var damagePopupScript = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopupScript.Setup(damage, isCritical);
    }

    private TextMeshPro textMesh;
    private Color textColor;
    private float disappearTimer = 1f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(string damage, bool isCritical)
    {
        textMesh.SetText(damage);

        textMesh.fontSize = 10;
        textMesh.color = Color.yellow;

        if (isCritical) {
            textMesh.fontSize = 14;
            textMesh.color = Color.red;
        }

        textColor = textMesh.color;
    }

    public void Update()
    {
        var speed = 1f;
        transform.Translate(new Vector3(1, 1) * speed * Time.deltaTime);

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0) {
            var disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}

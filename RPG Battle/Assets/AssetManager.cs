using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public Transform damagePopup;

    public static AssetManager i;

    private void Awake()
    {
        if (i == null) {
            i = this;
        }
    }
}

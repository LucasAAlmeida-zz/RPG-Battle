using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager i { get; private set; }

    public Material hero1Material;
    public Material enemy1Material;
    public Transform damagePopup;

    private void Awake()
    {
        if (i == null) {
            i = this;
        }
    }
}

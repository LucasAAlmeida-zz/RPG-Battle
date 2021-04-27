using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }

    public Material hero1Material;
    public Material enemy1Material;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
    }
}

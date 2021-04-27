using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private new MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material)
    {
        renderer.material = material;
    }

    public void PlayAttackAnimation(Vector3 attackDirection)
    {
        Debug.Log("Attack animation");
    }
}

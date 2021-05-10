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

    public void PlayIdleAnimation()
    {
        Debug.Log("Idle animation");
    }

    public void PlayAttackAnimation()
    {
        Debug.Log("Attack animation");
    }

    public void PlayMoveRightAnimation()
    {
        Debug.Log("Move right animation");
    }

    public void PlayMoveLeftAnimation()
    {
        Debug.Log("Move left animation");

    }
}

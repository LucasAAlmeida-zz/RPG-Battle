using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    CharacterAnimation characterAnimation;

    private State state;
    private Vector3 moveTargetPosition;
    private Action onMoveComplete;

    private enum State
    {
        Idle,
        Moving,
        Busy,
    }

    private void Awake()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        state = State.Idle;
    }

    public void Setup(bool isHero)
    {
        var material = (isHero) ? AssetManager.Instance.hero1Material : AssetManager.Instance.enemy1Material;
        characterAnimation.SetMaterial(material);
    }

    private void Update()
    {
        switch(state) {
            case State.Idle: break;
            case State.Moving:
                var moveSpeed = 10f;
                transform.position += (moveTargetPosition - GetPosition()) * moveSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if (Vector3.Distance(GetPosition(), moveTargetPosition) < reachedDistance) {
                    transform.position = moveTargetPosition;
                    onMoveComplete();
                }

                break;
            case State.Busy: break;
        }
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
    {
        MoveToPosition(targetCharacterBattle.GetPosition(), () => { });
        //var attackDirection = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
        //characterAnimation.PlayAttackAnimation(attackDirection);
        //onAttackComplete();
    }

    private void MoveToPosition(Vector3 moveTargetPosition, Action onMoveComplete)
    {
        this.moveTargetPosition = moveTargetPosition;
        this.onMoveComplete = onMoveComplete;
        state = State.Moving;
    }
}

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
    private bool isHero;

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
        this.isHero = isHero;
        var material = (isHero) ? AssetManager.Instance.hero1Material : AssetManager.Instance.enemy1Material;
        characterAnimation.SetMaterial(material);
        characterAnimation.PlayIdleAnimation();
    }

    private void Update()
    {
        switch(state) {
            case State.Idle: break;
            case State.Moving:
                var moveSpeed = 5f;
                transform.position += (moveTargetPosition - GetPosition()) * moveSpeed * Time.deltaTime;

                float reachedDistance = 0.2f;
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
        var moveTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized;
        var startingPosition = GetPosition();

        //Go to enemy position
        MoveToPosition(moveTargetPosition, () => {
            //Arrived at enemy, attack animation
            state = State.Busy;
            var attackDirection = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            characterAnimation.PlayAttackAnimation(attackDirection);

            // Go back to starting position
            MoveToPosition(startingPosition, () => {
                // Went back, back to idle
                state = State.Idle;
                characterAnimation.PlayIdleAnimation();
                onAttackComplete();
            });
        });
    }

    private void MoveToPosition(Vector3 moveTargetPosition, Action onMoveComplete)
    {
        this.moveTargetPosition = moveTargetPosition;
        this.onMoveComplete = onMoveComplete;
        state = State.Moving;
        if (moveTargetPosition.x > 0) {
            characterAnimation.PlayMoveRightAnimation();
        } else {
            characterAnimation.PlayMoveLeftAnimation();
        }
    }
}

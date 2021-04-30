using System;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private CharacterAnimation characterAnimation;
    private GameObject activeHighlight;

    private State state;
    private Vector3 moveTargetPosition;
    private Action onMoveComplete;

    private HealthManager healthManager;

    [SerializeField] int damageAmount = 500;

    private enum State
    {
        Idle,
        Moving,
        Busy,
    }

    private void Awake()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        activeHighlight = transform.Find("ActiveHighlight").gameObject;
    }

    public void Setup(bool isHero)
    {
        state = State.Idle;
        var material = (isHero) ? AssetManager.Instance.hero1Material : AssetManager.Instance.enemy1Material;
        characterAnimation.SetMaterial(material);
        characterAnimation.PlayIdleAnimation();
        ShowActiveHighlight(false);
        healthManager = new HealthManager(1000);
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

    private void TakeDamage(int damage)
    {
        healthManager.TakeDamate(damage);
        Debug.Log("Health: " + healthManager.GetHealth());
    }

    public bool IsDead()
    {
        return healthManager.IsDead();
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
            targetCharacterBattle.TakeDamage(damageAmount);

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

    public void ShowActiveHighlight(bool option)
    {
        activeHighlight.SetActive(option);
    }
}

using System;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip moveAudioClip;
    [SerializeField] private AudioClip attackMissAudioClip;
    [SerializeField] private AudioClip attackNormalAudioClip;
    [SerializeField] private AudioClip attackCritAudioClip;
    [SerializeField] private AudioClip dieAudioClip;

    private CharacterAnimation characterAnimation;
    private HealthBar healthBar;

    private State state;
    private Vector3 moveTargetPosition;
    private Action onMoveComplete;

    private HealthManager healthManager;
    private MeshRenderer meshRenderer;

    CharacterStats characterStats;
    private bool turnSpent = false;

    private enum State
    {
        Idle,
        Moving,
        Busy,
    }

    private void Awake()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(CharacterStats characterStats)
    {
        state = State.Idle;
        this.characterStats = characterStats;
        meshRenderer.material.color = characterStats.color;

        characterAnimation.PlayIdleAnimation();
        healthManager = new HealthManager(characterStats.maxHealth);
    }

    public CharacterStats GetCharacterStats()
    {
        return characterStats;
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
        healthBar.SetHealthPercent(healthManager.GetHealthPercent());

        if (IsDead()) {
            audioSource.PlayOneShot(dieAudioClip);

            meshRenderer.material.color = Color.grey;
            transform.Translate(Vector3.down * 0.5f);
        }
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
        this.MoveToPosition(moveTargetPosition, () => {
            //Arrived at enemy, attack animation
            state = State.Busy;
            HandleAttack(targetCharacterBattle);

            // Go back to starting position
            MoveToPosition(startingPosition, () => {
                // Went back, back to idle
                state = State.Idle;

                characterAnimation.PlayIdleAnimation();
                onAttackComplete();
            });
        });
    }

    private void HandleAttack(CharacterBattle targetCharacterBattle)
    {
        var attackDirection = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
        characterAnimation.PlayAttackAnimation(attackDirection);

        bool hasHit = UnityEngine.Random.Range(0, 100) < characterStats.accuracy;

        if (!hasHit) {
            audioSource.PlayOneShot(attackMissAudioClip);
            DamagePopup.Create(targetCharacterBattle.GetPosition(), "Miss");
            return;
        }

        var damage = (int)UnityEngine.Random.Range(characterStats.power * 0.9f, characterStats.power * 1.1f);
        bool isCritical = UnityEngine.Random.Range(0, 100) < characterStats.critChance;
        float shakeMagnitude;
        float shakeDuration;
        if (!isCritical) {
            audioSource.PlayOneShot(attackNormalAudioClip);
            shakeDuration = .2f;
            shakeMagnitude = .2f;
        } else {
            audioSource.PlayOneShot(attackCritAudioClip);
            damage = (int)(damage * 1.5);
            shakeDuration = .4f;
            shakeMagnitude = .4f;
        }
        StartCoroutine(CameraShake.Shake(shakeDuration, shakeMagnitude));
        targetCharacterBattle.TakeDamage(damage);
        DamagePopup.Create(targetCharacterBattle.GetPosition(), damage.ToString(), isCritical);
    }

    private void MoveToPosition(Vector3 moveTargetPosition, Action onMoveComplete)
    {
        audioSource.PlayOneShot(moveAudioClip, .2f);

        this.moveTargetPosition = moveTargetPosition;
        this.onMoveComplete = onMoveComplete;
        state = State.Moving;
        if (moveTargetPosition.x > 0) {
            characterAnimation.PlayMoveRightAnimation();
        } else {
            characterAnimation.PlayMoveLeftAnimation();
        }
    }

    public void SpendTurn()
    {
        meshRenderer.material.color = Color.gray;
        turnSpent = true;
    }

    public bool IsAvailableToAct()
    {
        return !turnSpent && !IsDead();
    }

    public void TryRefreshTurn()
    {
        if (!IsDead()) {
            meshRenderer.material.color = characterStats.color;
            turnSpent = false;
        }
    }
}

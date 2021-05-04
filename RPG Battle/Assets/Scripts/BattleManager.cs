using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Transform pfCharacterBattle;
    [SerializeField] private SelectionSpotlight heroSelectionSpotlight;
    [SerializeField] private SelectionSpotlight enemySelectionSpotlight;

    CharacterBattle heroMiddle;
    CharacterBattle heroLeft;
    CharacterBattle heroRight;
    CharacterBattle enemyMiddle;
    CharacterBattle enemyLeft;
    CharacterBattle enemyRight;

    CharacterBattle selectedHero;
    CharacterBattle selectedEnemy;

    private State state;

    private enum State
    {
        HeroesTurn,
        EnemiesTurn,
        Busy,
        BattleEnded,
    }

    private void Start()
    {
        var heroesStats = new CharacterStats[4] { 
            Resources.Load("CharacterStats/Heroes/Hero1") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/Hero2") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/Hero3") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/HeroBoss") as CharacterStats,
        };
        var enemiesStats = new CharacterStats[4] {
            Resources.Load("CharacterStats/Enemies/Enemy1") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/Enemy2") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/Enemy3") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/EnemyBoss") as CharacterStats,
        };

        var heroMiddlePos = new Vector3(-5, 0.5f, 0);
        var heroStat = heroesStats[UnityEngine.Random.Range(0, 4)];
        heroMiddle = SpawnCharacter(heroMiddlePos, heroStat);
        heroesStats = heroesStats.Where(h => h != heroStat).ToArray();

        var heroLeftPos = new Vector3(-6, 0.5f, 5);
        heroStat = heroesStats[UnityEngine.Random.Range(0, 3)];
        heroLeft = SpawnCharacter(heroLeftPos, heroStat);
        heroesStats = heroesStats.Where(h => h != heroStat).ToArray();

        var heroRightPos = new Vector3(-6, 0.5f, -5);
        heroStat = heroesStats[UnityEngine.Random.Range(0, 2)];
        heroRight = SpawnCharacter(heroRightPos, heroStat);
        heroesStats = heroesStats.Where(h => h != heroStat).ToArray();

        var enemyMiddlePos = new Vector3(5, 0.5f, 0);
        var enemyStat = enemiesStats[UnityEngine.Random.Range(0, 4)];
        enemyMiddle = SpawnCharacter(enemyMiddlePos, enemyStat);
        enemiesStats = enemiesStats.Where(h => h != enemyStat).ToArray();

        var enemyLeftPos = new Vector3(6, 0.5f, 5);
        enemyStat = enemiesStats[UnityEngine.Random.Range(0, 3)];
        enemyLeft = SpawnCharacter(enemyLeftPos, enemyStat);
        enemiesStats = enemiesStats.Where(h => h != enemyStat).ToArray();

        var enemyRightPos = new Vector3(6, 0.5f, -5);
        enemyStat = enemiesStats[UnityEngine.Random.Range(0, 2)];
        enemyRight = SpawnCharacter(enemyRightPos, enemyStat);
        enemiesStats = enemiesStats.Where(h => h != enemyStat).ToArray();

        state = State.HeroesTurn;

        ChangeSelectedHeroUp();
        ChangeSelectedEnemyUp();
    }

    private CharacterBattle SpawnCharacter(Vector3 characterPosition, CharacterStats characterStats)
    {
        var characterTransform = Instantiate(pfCharacterBattle, characterPosition, Quaternion.identity);
        var characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterStats);
        return characterBattle;
    }

    private void Update()
    {
        switch (state) {
            case State.HeroesTurn:
                HandleHeroesTurn();
                break;
            case State.EnemiesTurn:
                HandleEnemiesTurn();
                break;
            case State.BattleEnded:
                break;
        }
    }

    #region Heroes Turn
    private void HandleHeroesTurn()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            ChangeSelectedHeroUp();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            ChangeSelectedHeroDown();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            ChangeSelectedEnemyUp();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            ChangeSelectedEnemyDown();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (selectedHero.IsAvailableToAct() && !selectedEnemy.IsDead()) {
                state = State.Busy;
                selectedHero.Attack(selectedEnemy, () => {
                    OnHeroAttackComplete();
                });
            }
        }
    }

    private void ChangeSelectedHeroUp()
    {
        if (selectedHero == heroMiddle) {
            selectedHero = heroLeft;
        } else if (selectedHero == heroLeft) {
            selectedHero = heroRight;
        } else {
            selectedHero = heroMiddle;
        }

        if (selectedHero.IsAvailableToAct()) {
            heroSelectionSpotlight.SetTargetCharacter(selectedHero);
        } else {
            ChangeSelectedHeroUp();
        }
    }

    private void ChangeSelectedHeroDown()
    {
        if (selectedHero == heroMiddle) {
            selectedHero = heroRight;
        } else if (selectedHero == heroLeft) {
            selectedHero = heroMiddle;
        } else {
            selectedHero = heroLeft;
        }

        if (selectedHero.IsAvailableToAct()) {
            heroSelectionSpotlight.SetTargetCharacter(selectedHero);
        } else {
            ChangeSelectedHeroDown();
        }
    }

    private void ChangeSelectedEnemyUp()
    {
        if (selectedEnemy == enemyMiddle) {
            selectedEnemy = enemyLeft;
        } else if (selectedEnemy == enemyLeft) {
            selectedEnemy = enemyRight;
        } else {
            selectedEnemy = enemyMiddle;
        }

        if (!selectedEnemy.IsDead()) {
            enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
        } else {
            ChangeSelectedEnemyUp();
        }
    }

    private void ChangeSelectedEnemyDown()
    {
        if (selectedEnemy == enemyMiddle) {
            selectedEnemy = enemyRight;
        } else if (selectedEnemy == enemyLeft) {
            selectedEnemy = enemyMiddle;
        } else {
            selectedEnemy = enemyLeft;
        }

        if (!selectedEnemy.IsDead()) {
            enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
        } else {
            ChangeSelectedEnemyDown();
        }
    }

    private void OnHeroAttackComplete()
    {
        selectedHero.SpendTurn();

        if (enemyMiddle.IsDead() && enemyLeft.IsDead() && enemyRight.IsDead()) {
            state = State.BattleEnded;
            HandleBattleEnded();
            return;
        }

        state = State.EnemiesTurn;
        if (!enemyMiddle.IsAvailableToAct() && !enemyLeft.IsAvailableToAct() && !enemyRight.IsAvailableToAct()) {
            enemyMiddle.TryRefreshTurn();
            enemyLeft.TryRefreshTurn();
            enemyRight.TryRefreshTurn();
        }
        heroSelectionSpotlight.gameObject.SetActive(false);
        enemySelectionSpotlight.gameObject.SetActive(false);
    }
    #endregion

    #region Enemies Turn
    private void HandleEnemiesTurn()
    {
        CharacterBattle attackingEnemy = ChooseAttackingEnemy();
        CharacterBattle attackedHero = ChooseAttackedHero();

        state = State.Busy;
        attackingEnemy.Attack(attackedHero, () => {
            OnEnemyAttackComplete(attackingEnemy);
        });
    }

    private CharacterBattle ChooseAttackingEnemy()
    {
        var enemies = new CharacterBattle[3] { enemyMiddle, enemyLeft, enemyRight };
        CharacterBattle attackingEnemy;
        do {
            attackingEnemy = enemies[UnityEngine.Random.Range(0, 3)];
        } while (!attackingEnemy.IsAvailableToAct());
        return attackingEnemy;
    }

    private CharacterBattle ChooseAttackedHero()
    {
        var heroes = new CharacterBattle[3] { heroMiddle, heroLeft, heroRight };
        CharacterBattle attackedHero;

        do {
            attackedHero = heroes[UnityEngine.Random.Range(0, 3)];
        } while (attackedHero.IsDead());
        return attackedHero;
    }

    private void OnEnemyAttackComplete(CharacterBattle attackingEnemy)
    {
        attackingEnemy.SpendTurn();

        if (heroMiddle.IsDead() && heroLeft.IsDead() && heroRight.IsDead()) {
            state = State.BattleEnded;
            HandleBattleEnded();
            return;
        }

        state = State.HeroesTurn;
        if (!heroMiddle.IsAvailableToAct() && !heroLeft.IsAvailableToAct() && !heroRight.IsAvailableToAct()) {
            heroMiddle.TryRefreshTurn();
            heroLeft.TryRefreshTurn();
            heroRight.TryRefreshTurn();
        }
        ChangeSelectedHeroUp();

        heroSelectionSpotlight.gameObject.SetActive(true);
        enemySelectionSpotlight.gameObject.SetActive(true);
    }
    #endregion

    private void HandleBattleEnded()
    {
        Debug.Log("Battle Ended");
    }
}

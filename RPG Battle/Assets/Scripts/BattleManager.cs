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

    private void Update()
    {
        if (IsBattleOver()) {
            return;
        }

        switch (state) {
            case State.HeroesTurn:
                HandleHeroesTurn();
                break;
            case State.EnemiesTurn:
                HandleEnemiesTurn();
                break;
        }
    }

    private bool IsBattleOver()
    {
        return heroMiddle.IsDead() || enemyMiddle.IsDead();
    }

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
            state = State.Busy;
            selectedHero.Attack(selectedEnemy, () => {
                state = State.EnemiesTurn;
                selectedHero.SpendTurn();
                if (heroMiddle.IsTurnSpent() && heroLeft.IsTurnSpent() && heroRight.IsTurnSpent()) {
                    heroMiddle.RefreshTurn();
                    heroLeft.RefreshTurn();
                    heroRight.RefreshTurn();
                }
                ChangeSelectedHeroUp();
            });
        }
    }

    private void HandleEnemiesTurn()
    {
        CharacterBattle attackingEnemy = ChooseAttackingEnemy();
        CharacterBattle attackedHero = ChooseAttackedHero();

        state = State.Busy;
        attackingEnemy.Attack(attackedHero, () => {
            state = State.HeroesTurn;
            attackingEnemy.SpendTurn();
            if (enemyMiddle.IsTurnSpent() && enemyLeft.IsTurnSpent() && enemyRight.IsTurnSpent()) {
                enemyMiddle.RefreshTurn();
                enemyLeft.RefreshTurn();
                enemyRight.RefreshTurn();
            }
            ChangeSelectedEnemyUp();
        });
    }

    private CharacterBattle ChooseAttackedHero()
    {
        var heroes = new CharacterBattle[3] { heroMiddle, heroLeft, heroRight };
        var attackedHero = heroes[UnityEngine.Random.Range(0, 3)];
        return attackedHero;
    }

    private CharacterBattle ChooseAttackingEnemy()
    {
        var enemies = new CharacterBattle[3] { enemyMiddle, enemyLeft, enemyRight };
        CharacterBattle attackingEnemy;
        do {
            attackingEnemy = enemies[UnityEngine.Random.Range(0, 3)];
        } while (attackingEnemy.IsTurnSpent());
        return attackingEnemy;
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

        if (selectedHero.IsTurnSpent()) {
            ChangeSelectedHeroUp();
        } else {
            heroSelectionSpotlight.SetTargetCharacter(selectedHero);
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

        if (selectedHero.IsTurnSpent()) {
            ChangeSelectedHeroDown();
        } else {
            heroSelectionSpotlight.SetTargetCharacter(selectedHero);
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

        enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
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

        enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
    }

    private CharacterBattle SpawnCharacter(Vector3 characterPosition, CharacterStats characterStats)
    {
        var characterTransform = Instantiate(pfCharacterBattle, characterPosition, Quaternion.identity);
        var characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterStats);
        return characterBattle;
    }
}

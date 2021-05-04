using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Transform pfCharacterBattle;

    CharacterBattle heroMiddle;
    CharacterBattle heroLeft;
    CharacterBattle heroRight;
    CharacterBattle enemyMiddle;
    CharacterBattle enemyLeft;
    CharacterBattle enemyRight;

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
        heroMiddle.ShowActiveHighlight(true);
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            state = State.Busy;
            heroMiddle.Attack(enemyMiddle, () => {
                heroMiddle.ShowActiveHighlight(false);
                state = State.EnemiesTurn;
                enemyMiddle.ShowActiveHighlight(true);
            });
        }
    }

    private void HandleEnemiesTurn()
    {
        state = State.Busy;
        enemyMiddle.Attack(heroMiddle, () => {
            enemyMiddle.ShowActiveHighlight(false);
            state = State.HeroesTurn;
            heroMiddle.ShowActiveHighlight(true);
        });
    }

    private CharacterBattle SpawnCharacter(Vector3 characterPosition, CharacterStats characterStats)
    {
        var characterTransform = Instantiate(pfCharacterBattle, characterPosition, Quaternion.identity);
        var characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterStats);
        return characterBattle;
    }
}

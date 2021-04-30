using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Transform pfCharacterBattle;

    [SerializeField] private Vector3 hero1Pos = new Vector3(5, 0.5f, 0);
    [SerializeField] private Vector3 enemy1Pos = new Vector3(-5, 0.5f, 0);

    CharacterBattle hero;
    CharacterBattle enemy;

    private State state;

    private enum State
    {
        HeroesTurn,
        EnemiesTurn,
        Busy,
    }

    private void Start()
    {
        bool isHero = true;
        hero = SpawnCharacter(isHero);
        enemy = SpawnCharacter(!isHero);

        state = State.HeroesTurn;
        hero.ShowActiveHighlight(true);
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
        if (hero.IsDead()) {
            Debug.Log("Enemy wins!");
            return true;
        }

        if (enemy.IsDead()) {
            Debug.Log("Hero wins!");
            return true;
        }

        return false;
    }

    private void HandleHeroesTurn()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            state = State.Busy;
            hero.Attack(enemy, () => {
                hero.ShowActiveHighlight(false);
                state = State.EnemiesTurn;
                enemy.ShowActiveHighlight(true);
            });
        }
    }

    private void HandleEnemiesTurn()
    {
        state = State.Busy;
        enemy.Attack(hero, () => {
            enemy.ShowActiveHighlight(false);
            state = State.HeroesTurn;
            hero.ShowActiveHighlight(true);
        });
    }

    private CharacterBattle SpawnCharacter(bool isHero)
    {
        Vector3 position = (isHero) ? hero1Pos : enemy1Pos;
        var characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        var characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isHero);
        return characterBattle;
    }
}

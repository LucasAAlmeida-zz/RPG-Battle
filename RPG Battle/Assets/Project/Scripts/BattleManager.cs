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
    [SerializeField] private GameObject battleResultsWindow;

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
        SpawnHeroTeam();
        SpawnEnemyTeam();

        state = State.HeroesTurn;

        ChangeSelectedHeroUp();
        ChangeSelectedEnemyUp();
    }

    #region Spawn Characters

    #region Spawn Hero Team
    private void SpawnHeroTeam()
    {
        var heroTeamStats = GetHeroTeamStats();

        var heroMiddlePos = new Vector3(-5, 0.5f, 0);
        var heroLeftPos = new Vector3(-6, 0.5f, 5);
        var heroRightPos = new Vector3(-6, 0.5f, -5);

        heroMiddle = SpawnCharacter(heroMiddlePos, heroTeamStats[0]);
        heroLeft = SpawnCharacter(heroLeftPos, heroTeamStats[1]);
        heroRight = SpawnCharacter(heroRightPos, heroTeamStats[2]);
    }
    private List<CharacterStats> GetHeroTeamStats()
    {
        if (HeroTeam.i != null) {
            return HeroTeam.i.GetHeroTeam();
        }
        return CreateRandomHeroTeam();
    }
    private List<CharacterStats> CreateRandomHeroTeam()
    {
        var heroesStats = new CharacterStats[4] {
            Resources.Load("CharacterStats/Heroes/Hero1") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/Hero2") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/Hero3") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/HeroBoss") as CharacterStats,
        };

        return CreateRandomTeam(heroesStats);
    }
    #endregion

    #region Spawn EnemyTeam
    private void SpawnEnemyTeam()
    {
        var enemyTeamStats = CreateRandomEnemyTeam();

        var enemyMiddlePos = new Vector3(5, 0.5f, 0);
        var enemyLeftPos = new Vector3(6, 0.5f, 5);
        var enemyRightPos = new Vector3(6, 0.5f, -5);

        enemyMiddle = SpawnCharacter(enemyMiddlePos, enemyTeamStats[0]);
        enemyLeft = SpawnCharacter(enemyLeftPos, enemyTeamStats[1]);
        enemyRight = SpawnCharacter(enemyRightPos, enemyTeamStats[2]);
    }
    private List<CharacterStats> CreateRandomEnemyTeam()
    {
        var enemiesStats = new CharacterStats[4] {
            Resources.Load("CharacterStats/Enemies/Enemy1") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/Enemy2") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/Enemy3") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/EnemyBoss") as CharacterStats,
        };

        return CreateRandomTeam(enemiesStats);
    }
    #endregion

    private static List<CharacterStats> CreateRandomTeam(CharacterStats[] characterStatsList)
    {
        var characterTeam = new List<CharacterStats>();

        var characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Length)];
        characterStatsList = characterStatsList.Where(h => h != characterStats).ToArray();
        characterTeam.Add(characterStats);

        characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Length)];
        characterStatsList = characterStatsList.Where(h => h != characterStats).ToArray();
        characterTeam.Add(characterStats);

        characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Length)];
        characterStatsList = characterStatsList.Where(h => h != characterStats).ToArray();
        characterTeam.Add(characterStats);

        return characterTeam;
    }

    private CharacterBattle SpawnCharacter(Vector3 characterPosition, CharacterStats characterStats)
    {
        var characterTransform = Instantiate(pfCharacterBattle, characterPosition, Quaternion.identity);
        var characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterStats);
        return characterBattle;
    }

    #endregion

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

        ValidateSelectedHero();
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

        ValidateSelectedHero();
    }

    private void ValidateSelectedHero()
    {
        if (!selectedHero.IsAvailableToAct()) {
            ChangeSelectedHeroUp();
            return;
        }

        heroSelectionSpotlight.SetTargetCharacter(selectedHero);
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

        ValidateSelectedEnemy();
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

        ValidateSelectedEnemy();
    }

    private void ValidateSelectedEnemy()
    {
        if (selectedEnemy.IsDead()) {
            ChangeSelectedEnemyUp();
            return;
        }

        enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
    }

    private void OnHeroAttackComplete()
    {
        selectedHero.SpendTurn();

        if (enemyMiddle.IsDead() && enemyLeft.IsDead() && enemyRight.IsDead()) {
            state = State.BattleEnded;
            bool haveHeroesWon = true;
            HandleBattleEnded(haveHeroesWon);
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
            bool haveHeroesWon = false;
            HandleBattleEnded(haveHeroesWon);
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

    private void HandleBattleEnded(bool haveHeroesWon)
    {
        battleResultsWindow.GetComponent<BattleResultsWindow>().ChangeBattleResultsText(haveHeroesWon);
        battleResultsWindow.SetActive(true);
    }
}

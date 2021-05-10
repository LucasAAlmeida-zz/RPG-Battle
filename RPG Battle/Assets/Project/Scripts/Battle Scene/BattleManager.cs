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
    [SerializeField] private GameObject battleResultsWindowGameObject;
    [SerializeField] private StatsInfo battleStatsInfo;

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

        ChangeSelectedHeroUp();
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
        var heroesStatsList = new List<CharacterStats> {
            Resources.Load("CharacterStats/Heroes/RedHero") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/GreenHero") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/BlueHero") as CharacterStats,
            Resources.Load("CharacterStats/Heroes/BlackHero") as CharacterStats
        };
        return CreateRandomTeam(heroesStatsList);
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

        ChangeSelectedEnemyUp();
    }
    private List<CharacterStats> CreateRandomEnemyTeam()
    {
        var enemiesStatsList = new List<CharacterStats> {
            Resources.Load("CharacterStats/Enemies/CyanEnemy") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/MagentaEnemy") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/YellowEnemy") as CharacterStats,
            Resources.Load("CharacterStats/Enemies/WhiteEnemy") as CharacterStats
        };
        return CreateRandomTeam(enemiesStatsList);
    }
    #endregion

    private static List<CharacterStats> CreateRandomTeam(List<CharacterStats> characterStatsList)
    {
        var characterTeam = new List<CharacterStats>();
        CharacterStats characterStats;

        characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Count)];
        characterTeam.Add(characterStats);
        characterStatsList.Remove(characterStats);

        characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Count)];
        characterTeam.Add(characterStats);
        characterStatsList.Remove(characterStats);

        characterStats = characterStatsList[UnityEngine.Random.Range(0, characterStatsList.Count)];
        characterTeam.Add(characterStats);
        characterStatsList.Remove(characterStats);

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

        if (Input.GetKeyDown(KeyCode.Space) && AttackIsPossible()) {
            state = State.Busy;
            selectedHero.Attack(selectedEnemy, () => {
                OnHeroAttackComplete();
            });
        }
    }

    private bool AttackIsPossible()
    {
        return selectedHero.IsAvailableToAct() && !selectedEnemy.IsDead();
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

        if (!selectedHero.IsAvailableToAct()) {
            ChangeSelectedHeroUp();
            return;
        }

        UpdateHeroSpotlightTargetAndStatsInfo();
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

        if (!selectedHero.IsAvailableToAct()) {
            ChangeSelectedHeroDown();
            return;
        }

        UpdateHeroSpotlightTargetAndStatsInfo();
    }

    private void UpdateHeroSpotlightTargetAndStatsInfo()
    {
        heroSelectionSpotlight.SetTargetCharacter(selectedHero);
        battleStatsInfo.ChangeHeroStatsInfo(selectedHero.GetCharacterStats());
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

        if (selectedEnemy.IsDead()) {
            ChangeSelectedEnemyUp();
            return;
        }

        UpdateEnemySpotlightTargetAndStatsInfo();
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

        if (selectedEnemy.IsDead()) {
            ChangeSelectedEnemyDown();
            return;
        }

        UpdateEnemySpotlightTargetAndStatsInfo();
    }

    private void UpdateEnemySpotlightTargetAndStatsInfo()
    {
        enemySelectionSpotlight.SetTargetCharacter(selectedEnemy);
        battleStatsInfo.ChangeEnemyStatsInfo(selectedEnemy.GetCharacterStats());
    }

    private void OnHeroAttackComplete()
    {
        heroSelectionSpotlight.gameObject.SetActive(false);
        enemySelectionSpotlight.gameObject.SetActive(false);

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
        var availableEnemies = new List<CharacterBattle>();
        if (enemyMiddle.IsAvailableToAct()) {
            availableEnemies.Add(enemyMiddle);
        }
        if (enemyLeft.IsAvailableToAct()) {
            availableEnemies.Add(enemyLeft);
        }
        if (enemyRight.IsAvailableToAct()) {
            availableEnemies.Add(enemyRight);
        }
        return availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
    }

    private CharacterBattle ChooseAttackedHero()
    {
        var aliveHeroes = new List<CharacterBattle>();
        if (!heroMiddle.IsDead()) {
            aliveHeroes.Add(heroMiddle);
        }
        if (!heroLeft.IsDead()) {
            aliveHeroes.Add(heroLeft);
        }
        if (!heroRight.IsDead()) {
            aliveHeroes.Add(heroRight);
        }
        return aliveHeroes[UnityEngine.Random.Range(0, aliveHeroes.Count)];
    }

    private void OnEnemyAttackComplete(CharacterBattle attackingEnemy)
    {
        heroSelectionSpotlight.gameObject.SetActive(true);
        enemySelectionSpotlight.gameObject.SetActive(true);
        
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
    }
    #endregion

    private void HandleBattleEnded(bool haveHeroesWon)
    {
        Camera.main.gameObject.GetComponent<AudioSource>().Pause();
        battleResultsWindowGameObject.SetActive(true);
        var battleResultsWindow = battleResultsWindowGameObject.GetComponent<BattleResultsWindow>();
        battleResultsWindow.ChangeBattleResultsText(haveHeroesWon);
        battleResultsWindow.PlayBattleEndMusic(haveHeroesWon);
    }
}

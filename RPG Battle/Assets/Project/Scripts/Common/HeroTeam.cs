using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroTeam : MonoBehaviour
{
    List<CharacterStats> heroTeam;
    public static HeroTeam i;

    CharacterStats redHeroStats;
    CharacterStats greenHeroStats;
    CharacterStats blueHeroStats;
    CharacterStats blackHeroStats;

    private void Awake()
    {
        if (i == null) {
            i = this;
            DontDestroyOnLoad(gameObject);

            heroTeam = new List<CharacterStats>();

            redHeroStats = Resources.Load("CharacterStats/Heroes/RedHero") as CharacterStats;
            greenHeroStats = Resources.Load("CharacterStats/Heroes/GreenHero") as CharacterStats;
            blueHeroStats = Resources.Load("CharacterStats/Heroes/BlueHero") as CharacterStats;
            blackHeroStats = Resources.Load("CharacterStats/Heroes/BlackHero") as CharacterStats;
        }
    }

    public CharacterStats GetRedHeroStats()
    {
        return redHeroStats;
    }
    public CharacterStats GetGreenHeroStats()
    {
        return greenHeroStats;
    }
    public CharacterStats GetBlueHeroStats()
    {
        return blueHeroStats;
    }
    public CharacterStats GetBlackHeroStats()
    {
        return blackHeroStats;
    }

    public int AddRedHeroToTeam()
    {
        return AddHeroToTeam(redHeroStats);
    }
    public int AddGreenHeroToTeam()
    {
        return AddHeroToTeam(greenHeroStats);
    }
    public int AddBlueHeroToTeam()
    {
        return AddHeroToTeam(blueHeroStats);
    }
    public int AddBlackHeroToTeam()
    {
        return AddHeroToTeam(blackHeroStats);
    }

    private int AddHeroToTeam(CharacterStats characterStats)
    {
        heroTeam.Add(characterStats);

        if (heroTeam.Count == 3) {
            SceneManager.LoadScene("BattleScene");
        }

        return heroTeam.Count;
    }

    public List<CharacterStats> GetHeroTeam()
    {
        return heroTeam;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

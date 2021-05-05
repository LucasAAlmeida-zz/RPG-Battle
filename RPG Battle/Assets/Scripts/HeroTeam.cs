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
    CharacterStats whiteHeroStats;

    private void Awake()
    {
        if (i == null) {
            i = this;
            DontDestroyOnLoad(gameObject);

            heroTeam = new List<CharacterStats>();

            redHeroStats = Resources.Load("CharacterStats/Heroes/Hero1") as CharacterStats;
            greenHeroStats = Resources.Load("CharacterStats/Heroes/Hero2") as CharacterStats;
            blueHeroStats = Resources.Load("CharacterStats/Heroes/Hero3") as CharacterStats;
            whiteHeroStats = Resources.Load("CharacterStats/Heroes/HeroBoss") as CharacterStats;
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
    public CharacterStats GetWhiteHeroStats()
    {
        return whiteHeroStats;
    }

    public int AddRedCharacterToTeam()
    {
        return AddCharacterToTeam(redHeroStats);
    }
    public int AddGreenCharacterToTeam()
    {
        return AddCharacterToTeam(greenHeroStats);
    }
    public int AddBlueCharacterToTeam()
    {
        return AddCharacterToTeam(blueHeroStats);
    }
    public int AddWhiteCharacterToTeam()
    {
        return AddCharacterToTeam(whiteHeroStats);
    }

    private int AddCharacterToTeam(CharacterStats characterStats)
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
}

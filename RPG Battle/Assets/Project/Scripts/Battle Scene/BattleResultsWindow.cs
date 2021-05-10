using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class BattleResultsWindow : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI battleResultsText;
    [SerializeField] private AudioClip victoryTheme;
    [SerializeField] private AudioClip defeatTheme;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBattleResultsText(bool haveHeroesWon)
    {
        battleResultsText.text = (haveHeroesWon)
            ? "CONGRATULATIONS!\nYou have won the battle!"
            : "OH NO!\nIt seems you didn't train hard enough...";
    }

    public void OnAgainButtonClicked()
    {
        if (HeroTeam.i != null) {
            HeroTeam.i.SelfDestroy();
        }
        SceneManager.LoadScene("Menu");
    }

    public void PlayBattleEndMusic(bool haveHeroesWon)
    {
        if (haveHeroesWon) {
            audioSource.PlayOneShot(victoryTheme);
        } else {
            audioSource.PlayOneShot(defeatTheme);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleResultsWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI battleResultsText;

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
}

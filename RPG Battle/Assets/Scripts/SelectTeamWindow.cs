using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectTeamWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI accuracyText;

    public void RedHeroButtonMouseEnter()
    {
        var redHeroStats = Resources.Load("CharacterStats/Heroes/Hero1") as CharacterStats;
        DisplayHeroStats(redHeroStats);
    }

    public void GreenHeroButtonMouseEnter()
    {
        var greenHeroStats = Resources.Load("CharacterStats/Heroes/Hero2") as CharacterStats;
        DisplayHeroStats(greenHeroStats);
    }

    public void BlueHeroButtonMouseEnter()
    {
        var blueHeroStats = Resources.Load("CharacterStats/Heroes/Hero3") as CharacterStats;
        DisplayHeroStats(blueHeroStats);
    }

    public void HeroBossButtonMouseEnter()
    {
        var heroBossStats = Resources.Load("CharacterStats/Heroes/HeroBoss") as CharacterStats;
        DisplayHeroStats(heroBossStats);
    }

    public void RedHeroClicked()
    {
        Debug.Log("Hero clicked");
    }
    public void GreenHeroClicked()
    {
        Debug.Log("Hero clicked");
    }
    public void BlueHeroClicked()
    {
        Debug.Log("Hero clicked");
    }
    public void HeroBossClicked()
    {
        Debug.Log("Hero clicked");
    }

    private void DisplayHeroStats(CharacterStats heroStats)
    {
        nameText.text = "Name: " + heroStats.name;
        maxHealthText.text = "Max Health: " + heroStats.maxHealth.ToString();
        powerText.text = "Power: " + heroStats.power.ToString();
        critChanceText.text = "Crit Chance: " + heroStats.critChance.ToString();
        accuracyText.text = "Accuracy: " + heroStats.accuracy.ToString();
    }
}

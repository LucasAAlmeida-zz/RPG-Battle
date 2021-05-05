using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectTeamWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI accuracyText;

    [SerializeField] private GameObject redHeroPositionTextGameObject;
    [SerializeField] private GameObject greenHeroPositionTextGameObject;
    [SerializeField] private GameObject blueHeroPositionTextGameObject;
    [SerializeField] private GameObject whiteHeroPositionTextGameObject;

    public void RedHeroButtonMouseEnter()
    {
        DisplayHeroStats(HeroTeam.i.GetRedHeroStats());
    }
    public void GreenHeroButtonMouseEnter()
    {
        DisplayHeroStats(HeroTeam.i.GetGreenHeroStats());
    }
    public void BlueHeroButtonMouseEnter()
    {
        DisplayHeroStats(HeroTeam.i.GetBlueHeroStats());
    }
    public void WhiteHeroButtonMouseEnter()
    {
        DisplayHeroStats(HeroTeam.i.GetWhiteHeroStats());
    }

    private void DisplayHeroStats(CharacterStats heroStats)
    {
        nameText.text = "Name: " + heroStats.name;
        maxHealthText.text = "Max Health: " + heroStats.maxHealth.ToString();
        powerText.text = "Power: " + heroStats.power.ToString();
        critChanceText.text = "Crit Chance: " + heroStats.critChance.ToString();
        accuracyText.text = "Accuracy: " + heroStats.accuracy.ToString();
    }

    public void RedHeroClicked()
    {
        if (!redHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddRedCharacterToTeam();
            redHeroPositionTextGameObject.SetActive(true);
            redHeroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        }
        
    }
    public void GreenHeroClicked()
    {
        if (!greenHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddGreenCharacterToTeam();
            greenHeroPositionTextGameObject.SetActive(true);
            greenHeroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        }
    }
    public void BlueHeroClicked()
    {
        if (!blueHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddBlueCharacterToTeam();
            blueHeroPositionTextGameObject.SetActive(true);
            blueHeroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        }
    }
    public void WhiteHeroClicked()
    {
        if (!whiteHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddWhiteCharacterToTeam();
            whiteHeroPositionTextGameObject.SetActive(true);
            whiteHeroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        }
    }
}

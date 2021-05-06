using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectTeamWindow : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI accuracyText;

    [SerializeField] private GameObject redHeroPositionTextGameObject;
    [SerializeField] private GameObject greenHeroPositionTextGameObject;
    [SerializeField] private GameObject blueHeroPositionTextGameObject;
    [SerializeField] private GameObject whiteHeroPositionTextGameObject;

    [SerializeField] private AudioClip showHeroStatsAudioClip;
    [SerializeField] private AudioClip heroSelectedAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

        audioSource.PlayOneShot(showHeroStatsAudioClip);
    }

    public void RedHeroClicked()
    {
        if (!redHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddRedCharacterToTeam();
            HeroSelected(redHeroPositionTextGameObject, position);
        }
        
    }
    public void GreenHeroClicked()
    {
        if (!greenHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddGreenCharacterToTeam();
            HeroSelected(greenHeroPositionTextGameObject, position);
        }
    }

    public void BlueHeroClicked()
    {
        if (!blueHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddBlueCharacterToTeam();
            HeroSelected(blueHeroPositionTextGameObject, position);
        }
    }
    public void WhiteHeroClicked()
    {
        if (!whiteHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddWhiteCharacterToTeam();
            HeroSelected(whiteHeroPositionTextGameObject, position);
        }
    }

    private void HeroSelected(GameObject heroPositionTextGameObject, int position)
    {
        heroPositionTextGameObject.SetActive(true);
        heroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        audioSource.PlayOneShot(heroSelectedAudioClip);
    }
}

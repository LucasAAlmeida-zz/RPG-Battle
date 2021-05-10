using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectTeamWindow : MonoBehaviour
{
    private AudioSource audioSource;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI maxHealthText;
    private TextMeshProUGUI powerText;
    private TextMeshProUGUI critChanceText;
    private TextMeshProUGUI accuracyText;

    [SerializeField] private GameObject redHeroPositionTextGameObject;
    [SerializeField] private GameObject greenHeroPositionTextGameObject;
    [SerializeField] private GameObject blueHeroPositionTextGameObject;
    [SerializeField] private GameObject blackHeroPositionTextGameObject;

    [SerializeField] private AudioClip showHeroStatsAudioClip;
    [SerializeField] private AudioClip heroSelectedAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        nameText = transform.Find("HeroStatsInfo/NameText").GetComponent<TextMeshProUGUI>();
        maxHealthText = transform.Find("HeroStatsInfo/MaxHealthText").GetComponent<TextMeshProUGUI>();
        powerText = transform.Find("HeroStatsInfo/PowerText").GetComponent<TextMeshProUGUI>();
        critChanceText = transform.Find("HeroStatsInfo/CritChanceText").GetComponent<TextMeshProUGUI>();
        accuracyText = transform.Find("HeroStatsInfo/AccuracyText").GetComponent<TextMeshProUGUI>();
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
    public void BlackHeroButtonMouseEnter()
    {
        DisplayHeroStats(HeroTeam.i.GetBlackHeroStats());
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
            var position = HeroTeam.i.AddRedHeroToTeam();
            HeroSelected(redHeroPositionTextGameObject, position);
        }
        
    }
    public void GreenHeroClicked()
    {
        if (!greenHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddGreenHeroToTeam();
            HeroSelected(greenHeroPositionTextGameObject, position);
        }
    }

    public void BlueHeroClicked()
    {
        if (!blueHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddBlueHeroToTeam();
            HeroSelected(blueHeroPositionTextGameObject, position);
        }
    }
    public void BlackHeroClicked()
    {
        if (!blackHeroPositionTextGameObject.activeSelf) {
            var position = HeroTeam.i.AddBlackHeroToTeam();
            HeroSelected(blackHeroPositionTextGameObject, position);
        }
    }

    private void HeroSelected(GameObject heroPositionTextGameObject, int position)
    {
        heroPositionTextGameObject.SetActive(true);
        heroPositionTextGameObject.GetComponent<Text>().text = position.ToString();
        audioSource.PlayOneShot(heroSelectedAudioClip);
    }
}

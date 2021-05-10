using UnityEngine;
using TMPro;

public class StatsInfo : MonoBehaviour
{
    private TextMeshProUGUI heroNameText;
    private TextMeshProUGUI heroMaxHealthText;
    private TextMeshProUGUI heroCritChanceText;
    private TextMeshProUGUI heroPowerText;
    private TextMeshProUGUI heroAccuracyText;
    
    private TextMeshProUGUI enemyNameText;
    private TextMeshProUGUI enemyMaxHealthText;
    private TextMeshProUGUI enemyCritChanceText;
    private TextMeshProUGUI enemyPowerText;
    private TextMeshProUGUI enemyAccuracyText;

    private void Awake()
    {
        heroNameText = transform.Find("HeroSide/NameText").GetComponent<TextMeshProUGUI>();
        heroMaxHealthText = transform.Find("HeroSide/MaxHealthText").GetComponent<TextMeshProUGUI>();
        heroCritChanceText = transform.Find("HeroSide/CritChanceText").GetComponent<TextMeshProUGUI>();
        heroPowerText = transform.Find("HeroSide/PowerText").GetComponent<TextMeshProUGUI>();
        heroAccuracyText = transform.Find("HeroSide/AccuracyText").GetComponent<TextMeshProUGUI>();

        enemyNameText = transform.Find("EnemySide/NameText").GetComponent<TextMeshProUGUI>();
        enemyMaxHealthText = transform.Find("EnemySide/MaxHealthText").GetComponent<TextMeshProUGUI>();
        enemyCritChanceText = transform.Find("EnemySide/CritChanceText").GetComponent<TextMeshProUGUI>();
        enemyPowerText = transform.Find("EnemySide/PowerText").GetComponent<TextMeshProUGUI>();
        enemyAccuracyText = transform.Find("EnemySide/AccuracyText").GetComponent<TextMeshProUGUI>();
    }

    public void ChangeHeroStatsInfo(CharacterStats heroStats)
    {
        heroNameText.text = "Name: " + heroStats.name;
        heroMaxHealthText.text = "Max Health: " + heroStats.maxHealth;
        heroCritChanceText.text = "Crit Chance: " + heroStats.critChance;
        heroPowerText.text = "Power: " + heroStats.power;
        heroAccuracyText.text = "Accuracy: " + heroStats.accuracy;
    }

    public void ChangeEnemyStatsInfo(CharacterStats enemyStats)
    {
        enemyNameText.text = "Name: " + enemyStats.name;
        enemyMaxHealthText.text = "Max Health: " + enemyStats.maxHealth;
        enemyCritChanceText.text = "Crit Chance: " + enemyStats.critChance;
        enemyPowerText.text = "Power: " + enemyStats.power;
        enemyAccuracyText.text = "Accuracy: " + enemyStats.accuracy;
    }
}

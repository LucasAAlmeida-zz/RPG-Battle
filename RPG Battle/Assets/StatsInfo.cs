using UnityEngine;
using TMPro;

public class StatsInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private TextMeshProUGUI heroMaxHealthText;
    [SerializeField] private TextMeshProUGUI heroCritChanceText;
    [SerializeField] private TextMeshProUGUI heroPowerText;
    [SerializeField] private TextMeshProUGUI heroAccuracyText;

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyMaxHealthText;
    [SerializeField] private TextMeshProUGUI enemyCritChanceText;
    [SerializeField] private TextMeshProUGUI enemyPowerText;
    [SerializeField] private TextMeshProUGUI enemyAccuracyText;

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

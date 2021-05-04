using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    public new string name;
    public Color color;

    public int maxHealth;
    public int power;
    public int critChance;
    public int accuracy;
}

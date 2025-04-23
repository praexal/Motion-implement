using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina= 50;
    public float currentStamina;
    public float staminaRegen;

    public float attackCost;
    public float blockCost;
    public float blockDamage;

    public float damage = 10f;
}

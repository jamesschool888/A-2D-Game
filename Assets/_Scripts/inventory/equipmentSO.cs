using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class equipmentSO : ScriptableObject
{
    public string itemName;
    public int defense, agility;
    public float attack, critRate, critDamage;
    [SerializeField]
    private Sprite itemSprite;

    public void previewEquipment()
    {
        GameObject.Find("statManager").GetComponent<playerStats>().
        previewEquipmentStats(attack, defense, agility, critRate, critDamage, itemSprite);
    }

    public void equipItem()
    {
        playerStats playerStats = GameObject.Find("statManager").GetComponent<playerStats>();
        playerCombat playerAttack = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        Health playerDefense = GameObject.FindWithTag("Player").GetComponent<Health>();
        Health playerAgility = GameObject.FindWithTag("Player").GetComponent<Health>();
        playerCombat playerCritRate = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        playerCombat playerCritDMG = GameObject.FindWithTag("Player").GetComponent<playerCombat>();

        playerAttack.baseAttack += (attack / 100);
        playerDefense.baseDefense += defense;
        playerAgility.baseAgility += agility;
        playerCritRate.critChance += critRate;
        playerCritDMG.critDamageMultiplier += critDamage;

        playerStats.updateEquipmentStats();
        Debug.Log(playerAttack.baseAttack);

    }

    public void unequipItem()
    {
        playerStats playerStats = GameObject.Find("statManager").GetComponent<playerStats>();
        playerCombat playerAttack = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        Health playerDefense = GameObject.FindWithTag("Player").GetComponent<Health>();
        Health playerAgility = GameObject.FindWithTag("Player").GetComponent<Health>();
        playerCombat playerCritRate = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        playerCombat playerCritDMG = GameObject.FindWithTag("Player").GetComponent<playerCombat>();

        playerAttack.baseAttack -= (attack / 100);
        playerDefense.baseDefense -= defense;
        playerAgility.baseAgility -= agility;
        playerCritRate.critChance -= critRate;
        playerCritDMG.critDamageMultiplier -= critDamage;

        playerStats.updateEquipmentStats();
        Debug.Log(playerAttack.baseAttack);
    }
    
}

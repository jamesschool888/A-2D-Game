using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class itemSO : ScriptableObject
{
    public string itemName;
    public StatTochange statTochange = new StatTochange();
    public int amountToChangeStat;

    public AttributeTochange attributeTochange = new AttributeTochange();
    public int amountToChangeAttribute;

    
    public bool useItem()
    {
        if(statTochange == StatTochange.health)
        {
            Health playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            if (playerHealth.currentHealth.Value == 1)
            {
                return false;
            }
            else
            {
                playerHealth.addHealth(amountToChangeStat);
                return true;
            }
            
        }
        return false;
    }

    public enum StatTochange
    {
        none,
        health,
        mana,
        stamina
    };

    public enum AttributeTochange
    {
        none,
        Attack,
        defense,
        agility
    };
}

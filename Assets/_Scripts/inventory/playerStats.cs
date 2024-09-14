using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class playerStats : MonoBehaviour
{
    public int defense, agility;
    public float attack, critRate, critDamage;

    [SerializeField]
    private TMP_Text attackText, defenseText, agilityText, critRateText, critDamageText;

    [SerializeField]
    private TMP_Text attackPreText, defensePreText, agilityPreText, critRatePreText, critDamagePreText;

    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private GameObject selectedItemStats;

    [SerializeField]
    private GameObject selectedItemImage;

    void Awake()
    {
        updateEquipmentStats();
    }

    void Start()
    {
        updateEquipmentStats();
        Debug.Log(attack);
    }

    public void updateEquipmentStats()
    {
        playerCombat playerAttack = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        Health playerDefense = GameObject.FindWithTag("Player").GetComponent<Health>();
        Health playerAgility = GameObject.FindWithTag("Player").GetComponent<Health>();
        playerCombat playerCritRate = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        playerCombat playerCritDMG = GameObject.FindWithTag("Player").GetComponent<playerCombat>();
        string formattedCritRate = critRate.ToString("F2");
        string formattedCritDamage = critDamage.ToString("F2");

        defense = playerDefense.baseDefense;
        attack = playerAttack.baseAttack;
        agility = playerAgility.baseAgility;
        critRate = playerCritRate.critChance * 100;
        critDamage = playerCritDMG.critDamageMultiplier * 100;

        attackText.text = (attack * 100).ToString();
        defenseText.text = defense.ToString();
        agilityText.text = agility.ToString();
        critRateText.text = formattedCritRate.ToString() + '%';
        critDamageText.text = formattedCritDamage.ToString() + '%';
    }
    
    public void previewEquipmentStats(float attack, int defense, int agility, float critRate, float critDamage, Sprite itemSprite)
    {
        attackPreText.text = (attack * 100).ToString();
        defensePreText.text = defense.ToString();
        agilityPreText.text = agility.ToString();
        critRatePreText.text = (critRate * 100).ToString() + '%';
        critDamagePreText.text = (critDamage * 100).ToString() + '%';

        previewImage.sprite = itemSprite;

        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
    }

    public void turnOffPreviewStats()
    {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
    }
}
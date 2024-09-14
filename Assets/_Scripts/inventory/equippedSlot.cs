using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class equippedSlot : MonoBehaviour, IPointerClickHandler
{
    //SLOT APPEARANCE
    [SerializeField]
    private Image slotImage;

    [SerializeField]
    private TMP_Text slotName;

    //SLOT DATA
    [SerializeField]
    private ItemType itemType = new ItemType();

    private Sprite itemSprite;
    private string itemName;
    private string itemDescription;

    private inventoryManager inventoryManager;
    private equipmentSOLibrary equipmentSOLibrary;

    private void Start()
    {
        inventoryManager = GameObject.Find("inventoryCanvas").GetComponent<inventoryManager>(); 
        equipmentSOLibrary = GameObject.Find("inventoryCanvas").GetComponent<equipmentSOLibrary>();
    }

    //OTHER VARIABLES
    private bool slotInUse;
    [SerializeField]
    public GameObject selectedShader;

    [SerializeField]
    public bool thisItemSelected;

    [SerializeField]
    private Sprite emptySprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick();
        }
    }

    void onLeftClick()
    {
        if(thisItemSelected && slotInUse)
            unequipGear();
        else
        {
            inventoryManager.deselectAllSlots(); 
            selectedShader.SetActive(true);
            thisItemSelected = true;

            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                {
                    equipmentSOLibrary.equipmentSO[i].previewEquipment();
                }
            }
        }
    }

    void onRightClick()
    {
        unequipGear();
    }

    public void equipGear(Sprite itemSprite, string itemName, string itemDescription)
    {
        if(slotInUse)
            unequipGear();

        //update img
        this.itemSprite = itemSprite;
        slotImage.sprite = this.itemSprite;
        slotName.enabled = false;

        //update data
        this.itemName = itemName;
        this.itemDescription = itemDescription;

        //update player stats
        for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
        {
            if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                equipmentSOLibrary.equipmentSO[i].equipItem();
        }

        slotInUse = true;
        Debug.Log("Equipped " + this.itemName);
    }

    public void unequipGear()
    {
        inventoryManager.deselectAllSlots();

        inventoryManager.addItem(itemName, 1, itemSprite, itemDescription, itemType);

        this.itemSprite = emptySprite;
        slotImage.sprite = this.emptySprite;
        slotName.enabled = true;

        //update player stats
        for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
        {
            if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                equipmentSOLibrary.equipmentSO[i].unequipItem();
        }

        slotInUse = false;
        Debug.Log("Unequipped " +  this.itemName);

        GameObject.Find("statManager").GetComponent<playerStats>().turnOffPreviewStats();
    }
}

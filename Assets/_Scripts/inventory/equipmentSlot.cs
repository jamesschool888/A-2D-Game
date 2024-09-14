using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class equipmentSlot : MonoBehaviour, IPointerClickHandler
{
    // ITEM DATA
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;

    //ITEM SLOT
    [SerializeField]
    private Image itemImage;

    //EQUIPPED SLOT
    [SerializeField]
    private equippedSlot headSlot, armorSlot, legSlot, feetSlot,
        weaponSlot, neckSlot, ringSlot, armSlot;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private inventoryManager inventoryManager;
    private equipmentSOLibrary equipmentSOLibrary;

    private void Start()
    {
        inventoryManager = GameObject.Find("inventoryCanvas").GetComponent<inventoryManager>();
        equipmentSOLibrary = GameObject.Find("inventoryCanvas").GetComponent<equipmentSOLibrary>();
    }

    public int addItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        //check to see if the slot is already full
        if (isFull)
            return quantity;

        this.itemType = itemType;

        this.itemName = itemName;

        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        this.itemDescription = itemDescription;

        this.quantity = 1;
        isFull = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick();
        }
    }

    public void onLeftClick()
    {
        if (isFull)
        {
            if (thisItemSelected)
            {
                equipGear();

            }

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
        else
        {
            GameObject.Find("statManager").GetComponent<playerStats>().turnOffPreviewStats();
            inventoryManager.deselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
    }

    private void equipGear()
    {
        if(itemType == ItemType.head)
            headSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.armor)
            armorSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.pants)
            legSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.boots)
            feetSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.weapon)
            weaponSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.neck)
            neckSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.ring)
            ringSlot.equipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.arm)
            armSlot.equipGear(itemSprite, itemName, itemDescription);

        emptySlot();
    }

    private void emptySlot()
    {   
        itemImage.sprite = emptySprite;
        isFull = false;
    }

    public void onRightClick()
    {
        //create a new item right beside the player and subtract 
        GameObject itemToDrop = new GameObject(itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemName = itemName;
        newItem.sprite = itemSprite;
        newItem.itemDescription = itemDescription;
        newItem.itemType = itemType;

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;
        sr.sortingOrder = 5;
        sr.sortingLayerName = "Ground";

        //adding a collider
        BoxCollider2D collider = itemToDrop.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        //set the location
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(2, 0, 0);
        itemToDrop.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        //subtract the item
        this.quantity -= 1;
        if (this.quantity <= 0)
            emptySlot();
    }
}

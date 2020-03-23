﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory inventory;
    public List<Key> keys;
    public List<Weapon> weapons;
    public List<ConsumableItem> items;
    public List<Armor> armors;

    public ItemDataBase itemDataBase;

    private void Awake()
    {
        if(inventory == null)
        {
            inventory = this;
        }
        else if(inventory !=this)
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadInventory();
    }

    void LoadInventory()
    {
        for (int i = 0; i < GameManager.gm.weaponId.Length; i++)
        {
            AddWeapon(itemDataBase.GetWeapon(GameManager.gm.weaponId[i]));
        }

        for (int i = 0; i < GameManager.gm.keyId.Length; i++)
        {
            AddKey(itemDataBase.GetKey(GameManager.gm.keyId[i]));
        }

        for (int i = 0; i < GameManager.gm.itemId.Length; i++)
        {
            AddItem(itemDataBase.GetConsumableItem(GameManager.gm.itemId[i]));
        }

        for (int i = 0; i < GameManager.gm.armorId.Length; i++)
        {
            AddArmor(itemDataBase.GetArmor(GameManager.gm.armorId[i]));
        }
    }

    public void AddArmor(Armor armor)
    {
        armors.Add(armor);
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    public void AddKey(Key key)
    {
        keys.Add(key);
    }

    public bool CheckKey(Key key)
    {
        foreach( Key x in keys)
        {
            if(x == key)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(ConsumableItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(ConsumableItem item)
    {
        foreach( ConsumableItem x in items )
        {
            if(x == item)
            {
                items.Remove(x);
                break;
            }
        }
    }

    public int CountItem(ConsumableItem item)
    {
        int numberOfItem = 0;
        for(int i = 0; i < items.Count; i++)
        {
            if (item == items[i])
            {
                numberOfItem++;
            }
        }
        return numberOfItem;
    }
}

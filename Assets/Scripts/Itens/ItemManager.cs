using Itens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Itens
    {
    public enum ItemType
    {
        Coin,
        LifePack,
        Bomb
    }

}




public class ItemManager : Singleton<ItemManager>
{

    public List<ItemSetup> itemSetups;


    private void Start()
    {
        Reset();

    }   


    private void Reset()
    {
        foreach (var item in itemSetups)
        {
            item.soInt.value = 0;
        }

    }

    public void AddByType(ItemType itemType, int amount)
    {         
        itemSetups.Find(i => i.itemType == itemType).soInt.value += amount;
    }

    public void RemoveByType(ItemType itemType, int amount)
    {
        if (amount < 0) return;
        itemSetups.Find(i => i.itemType == itemType).soInt.value -= amount;
    }



}

[System.Serializable]

public class ItemSetup
{
    public ItemType itemType;
    public SOInt soInt;
    
}

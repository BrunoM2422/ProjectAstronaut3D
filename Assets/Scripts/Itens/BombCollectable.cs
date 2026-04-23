using Itens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectable : ItemCollectableBase
{
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddByType(ItemType.Bomb, 1);
    }
}

using System;
using UnityEngine;

public class OreMining : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ItemDataSO[] _oreTypes;
    
    public void MineOre(ItemDataSO ore)
    {
        _wallet.Add(ore.value);
    }
}
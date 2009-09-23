using UnityEngine;
using System.Collections;

public enum ItemType
{
    ApplierSpeed,
    ApplierGodMode,
    ApplierRemoteBomb,
    ApplierSize,
    ApplierAddBomb
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float value;
    public float time;
}
using UnityEngine;
using System.Collections;

public enum ItemType
{
    ApplierSpeed,
    ApplierGodMode,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float value;
    public float time;
}
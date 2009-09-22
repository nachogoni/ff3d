using UnityEngine;
using System.Collections;

public enum ItemType
{
    ApplierSpeed,
    ApplierGodMode,
    ApplierRemoteBomb,
}

public class Item : MonoBehaviour
{
    [HideInInspector]
    public ItemType itemType;
    [HideInInspector]
    public float value;
    [HideInInspector]
    public float time;
}
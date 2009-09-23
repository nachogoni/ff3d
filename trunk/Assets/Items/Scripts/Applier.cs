using UnityEngine;
using System.Collections;

public class Applier : MonoBehaviour
{
    [HideInInspector]
    public Actor actor;
    public float value;
    public float maxTime;

    public void showText(string value)
    {
        //Creo el texto
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Items/ItemText", typeof(GameObject)));
        go.transform.position = transform.position + Vector3.forward;
        ItemText itext = go.GetComponent(typeof(ItemText)) as ItemText;
        itext.value = value;
    }
}


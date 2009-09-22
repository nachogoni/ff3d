using UnityEngine;
using System.Collections;

/*public enum ActoryType
{
    ActorEnemy,
    ActorPlayer,
}*/

public class Actor : MonoBehaviour {

    // Vida del Actor: [Actual, Maxima]
    public float[] health = new float[2];
    // Velocidad del Actor
    public float speed;
    // Cantidad de Bombas que tiene para colocar
    public int bombCount;
    // Cantidad de celdas que ocupa una bomba
    public int bombSize;
    // Cantidad de dano que produce
    public float damage;
    // Puntaje que otorga al morir
    public float points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

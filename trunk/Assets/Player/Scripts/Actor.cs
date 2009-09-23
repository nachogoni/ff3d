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
    public int[] bombCount = new int[]{10,10,10};
    // Cantidad de celdas que ocupa una bomba
    public int bombSize;
    // tipo de bomba que tiene cargado el actor
    public BombType bomb = BombType.RemoteBomb;
	// Use this for initialization
}

using UnityEngine;
using System.Collections;

public class Maps : MonoBehaviour {

    public int actualLevel = 1;
    [HideInInspector]
    public GameObject[] walls;

    // Mapa del nivel
    int[] level = new int[] { 
        2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,
        2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,2,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,2,
        2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2
    };

    Object[] wallPrefabs;
    Object[] itemPrefabs;
    Object[] enemyPrefabs;

	// Use this for initialization
	void Start () {
        float rnd;
        GameObject go;

        // Genero el espacio donde van las referencias a las paredes
        walls = new GameObject[level.Length];

        // Levanto los prefabs
        itemPrefabs = Resources.LoadAll("Prefabs/Items", typeof(GameObject));
        wallPrefabs = Resources.LoadAll("Prefabs/Walls", typeof(GameObject));
        enemyPrefabs = Resources.LoadAll("Prefabs/Enemies", typeof(GameObject));
        
        // Genera Niveles dinamicos
        for (int i = 0; i < level.Length; i++)
        {
            //Si es un espacio en blanco
            if (level[i] == 0)
            {
                rnd = Random.RandomRange(0f, 1f);
                //Probabilidad de que haya una pared
                if (rnd < 0.5f)
                {
                    level[i] = 1;
                }
                //Probabilidad de que haya un enemigo
                else if (rnd > 0.97f)
                {
                    level[i] = 3;
                }
            }
        }

        level[811] = 0;
        level[812] = 0;
        level[813] = 0;
        level[783] = 0;
        level[781] = 0;

        // Creo el nivel
        for (int i = 0; i < level.Length; i++)
        {
            float posx = i % 30 - 14f;
            float posz = i / 30 - 14f;
            int index = level[i];

            if (index < 3)
            {
                go = (GameObject)GameObject.Instantiate(wallPrefabs[index], new Vector3(posx, 0.5f, posz), Quaternion.identity);
                go.transform.parent = gameObject.transform;
                walls[i] = (index == 0)?null:go;

                //Si es una pared y la probabilidad de que haya un item
                if ((index == 1) && ((rnd = Random.RandomRange(0f, 1f)) > 0.7f))
                {
                    //Elijo rnd el item
                    rnd = Random.RandomRange(0f, 1f) * itemPrefabs.Length;
                    go = (GameObject)GameObject.Instantiate(itemPrefabs[(int)rnd], new Vector3(posx, 0f, posz), Quaternion.identity);
                    go.transform.parent = gameObject.transform;
                }
            }
            else
            {
                go = (GameObject)GameObject.Instantiate(enemyPrefabs[index - 3], new Vector3(posx, 0.5f, posz), Quaternion.identity);
                go.transform.parent = gameObject.transform;
                walls[i] = null;    

                //agrego al enemy al gamecontrolles
                GameController.addEnemy();

            }

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
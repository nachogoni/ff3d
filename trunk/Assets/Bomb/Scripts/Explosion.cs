using UnityEngine;
using System.Collections;

public class Explosion : Damageable
{
    public int size = 1;
    public float time = .001f;

    [HideInInspector]
    public GameObject[] walls;

    // Use this for initialization
    void Start()
    {
        //GameObject go;
        bool seguir = true;
            
        GameObject go;
        Wall wall;
        int index, iZ = 0, iX = 0;
        
        for (int j = 0; j < 4; j++)
        {
            switch (j)
            {
                case 0: iZ =  0; iX = -1; break;
                case 1: iZ =  0; iX = +1; break;
                case 2: iZ = -1; iX =  0; break;
                case 3: iZ = +1; iX =  0; break;
            }

            for (int i = 1; i < size + 1 && seguir; i++)
            {
                go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ExplosionParticles", typeof(GameObject)));
                go.transform.position = transform.position + new Vector3((float)iX * i, 0f, (float)iZ * i);
                go.transform.parent = transform;

                index = ((int)transform.position.z + 14 + iZ * i) * 30 + ((int)transform.position.x + 14 + iX * i);

                if (walls[index] != null && (wall = (Wall)walls[index].GetComponent(typeof(Wall))) != null)
                {
                    walls[index] = (GameObject)wall.Destroy();
                    seguir = false;
                }
            }
            seguir = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            Object.Destroy(gameObject);
        }

    }
}
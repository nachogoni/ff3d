using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {

    public GameObject to;
    public int spritesTotalCount = 1;
    public int[] spritesSequenceUp = new int[] { 0 };
    public int[] spritesSequenceDown = new int[] { 0 };
    public int[] spritesSequenceLeft = new int[] { 0 };
    public int[] spritesSequenceRight = new int[] { 0 };
    public int[] spritesSequenceStay = new int[] { 0 };

    int i = 0, sequenceCount;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool done = false;
        float pos;
        int[] spritesSequence;

        float posX = gameObject.transform.position.x;
        float posZ = gameObject.transform.position.z;
        float velX = gameObject.rigidbody.velocity.x;
        float velZ = gameObject.rigidbody.velocity.z;

        if (velZ > 0.01f)
        {
            spritesSequence = spritesSequenceUp;
            pos = posZ;
        }
        else if (velX > 0.01f)
        {
            spritesSequence = spritesSequenceRight;
            pos = posX;
        }
        else if (velZ < -0.01f)
        {
            spritesSequence = spritesSequenceDown;
            pos = posZ;
        }
        else if (velX < -0.01f)
        {
            spritesSequence = spritesSequenceLeft;
            pos = posX;
        }
        else
        {
            spritesSequence = spritesSequenceStay;
            pos = 0f;
        }

        sequenceCount = spritesSequence.Length;




        for (int i = 0; !done && (i < sequenceCount); i++)
        {
            if (Mathf.Abs(pos % 1) < ((i + 1) / (float)sequenceCount))
            {
                done = true;
                to.renderer.material.mainTextureOffset = new Vector2((spritesSequence[i] * 1f) / (float)spritesTotalCount, 0);
                to.renderer.material.mainTextureScale = new Vector2(1f / (float)spritesTotalCount, 1);
            }
        }
    }
}

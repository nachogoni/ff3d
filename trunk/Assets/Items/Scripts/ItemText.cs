using UnityEngine;
using System.Collections;

public class ItemText : MonoBehaviour {

    public string value;
    float time, totaltime;
    const float PAUSE_TIME = 0.06f;
    const float END_TIME = 0.8f;
    TextMesh gotext = null;

	// Use this for initialization
	void Start () {
        gotext = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;
        gotext.text = value;
	}
	
	// Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        totaltime += Time.deltaTime;
        if (time > PAUSE_TIME)
        {
            time -= PAUSE_TIME;
            gotext.transform.position += Vector3.forward * Time.deltaTime * 5;
        }
        
        if (totaltime > END_TIME)
            Object.Destroy(gameObject);
    }
}

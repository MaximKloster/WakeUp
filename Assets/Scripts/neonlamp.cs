using UnityEngine;
using System.Collections;

public class neonlamp : MonoBehaviour
{
    public bool flackern = false;
	public bool on = true;
    // Use this for initialization
    void Start()
    {
		light.enabled = on;  
    }

    // Update is called once per frame
    void Update()
    {
		if (flackern) {
						if (Random.value > 0.9) { //a random chance
								if (light.enabled == true) { //if the light is on...
										light.enabled = false; //turn it off
								} else {
										light.enabled = true; //turn it on
								}
						}

				}
    }

  
}

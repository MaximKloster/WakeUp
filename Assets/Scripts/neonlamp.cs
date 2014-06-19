using UnityEngine;
using System.Collections;

public class neonlamp : MonoBehaviour
{
    public bool flackern = false;

    // Use this for initialization
    void Start()
    {
        light.active = true;
    }

    // Update is called once per frame
    void Update()
    {
      if (flackern) {
            light.active = false;
            randomwait();
            light.active = true;

        }


    }

    IEnumerator randomwait()
    {
         yield return new WaitForSeconds(Random.value);

    }
}

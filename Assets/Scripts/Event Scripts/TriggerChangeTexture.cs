using UnityEngine;
using System.Collections;

public class TriggerChangeTexture : MonoBehaviour
{
    [SerializeField]
    Texture changeToTexture = null;

    Texture startTexture;

    void Start()
    {
        startTexture = renderer.material.mainTexture;
    }

    void OnTriggerEnter(Collider other)
    {
        renderer.material.mainTexture = changeToTexture;
    }
}

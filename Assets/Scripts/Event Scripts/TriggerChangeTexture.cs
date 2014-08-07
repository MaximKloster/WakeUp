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
        StartCoroutine(ShowtextureOverTime());
    }

    IEnumerator ShowtextureOverTime()
    {
        renderer.material.mainTexture = changeToTexture;

        yield return new WaitForSeconds(0.15f);

        gameObject.SetActive(false);
    }
}

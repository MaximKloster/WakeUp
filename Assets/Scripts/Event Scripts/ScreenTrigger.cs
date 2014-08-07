using UnityEngine;
using System.Collections;

public class ScreenTrigger : MonoBehaviour
{
    [SerializeField]
    Texture startScreenTexture, endScreenTexture, gameOverTexture;

    //Vector3 playerStartposition;
    //Transform playerTransform;
    GUITexture screen;
    bool ingame;

    void Start()
    {
        screen = GameObject.Find("Screen").GetComponent<GUITexture>();

        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //playerStartposition = playerTransform.position;
        //playerTransform.position = new Vector3(1000, 1000, 1000);
        screen.texture = startScreenTexture;
    }

    void Update()
    {
        if (!ingame && Input.GetKeyDown(KeyCode.Space))
        {
            ingame = true;
            //playerTransform.position = playerStartposition;
            StartCoroutine(HideScreen());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            screen.texture = endScreenTexture;
            screen.gameObject.SetActive(true);

            StartCoroutine(ShowScreen());
        }
    }

    public void OnPlayerDead()
    {
        screen.texture = gameOverTexture;
        screen.gameObject.SetActive(true);

        StartCoroutine(ShowScreen());
    }

    IEnumerator ShowScreen()
    {
        while (screen.color.a < 0.95f)
        {
            screen.color = Color.Lerp(screen.color, Color.gray, Time.deltaTime);
            yield return null;
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(1000, 1000, 1000);
    }

    IEnumerator HideScreen()
    {
        yield return new WaitForSeconds(1);

        while (screen.color.a > 0.05f)
        {
            screen.color = Color.Lerp(screen.color, Color.clear, Time.deltaTime);
            yield return null;
        }

        screen.gameObject.SetActive(false);
    }
}

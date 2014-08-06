using UnityEngine;
using System.Collections;

public class ScreenTrigger : MonoBehaviour
{
    enum Screens
    {
        StartScreen,
        Endscreen, 
        GameOverScreen
    }

    [SerializeField]
    Screens screenTrigger = Screens.StartScreen;

    [SerializeField]
    Texture startScreenTexture, endScreenTexture, gameOverTexture;

    Transform player;

    GUITexture screen;

    void Start()
    {
        screen = GameObject.Find("Screen").GetComponent<GUITexture>();
        screen.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            screen.texture = screenTrigger == Screens.StartScreen ? startScreenTexture :
                screenTrigger == Screens.Endscreen ? endScreenTexture : gameOverTexture;

            screen.gameObject.SetActive(true);
            player = other.transform;
            StartCoroutine(ShowScreen());
        }
    }

    IEnumerator ShowScreen()
    {
        while (screen.color.a < 0.95)
        {
            screen.color = Color.Lerp(screen.color, Color.gray, Time.deltaTime);
            yield return null;
        }
    }
}

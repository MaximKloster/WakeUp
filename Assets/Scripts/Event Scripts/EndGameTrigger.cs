using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    bool endGame;
    Transform player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.transform;
            endGame = true;
        }
    }

    void Update()
    {
        if (endGame)
        {
            //var endSprite = player.FindChild("OVRCameraController").FindChild("End Sprite").GetComponent<SpriteRenderer>();
            //endSprite.color = Color.Lerp(endSprite.color, new Color(1, 1, 1, 1), 1f * Time.deltaTime);


        }
    }
}

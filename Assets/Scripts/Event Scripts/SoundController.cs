using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    AudioSource eventSounds;

    [SerializeField]
    int eventCount = 1;

    bool soundActive = false;

    // Update is called once per frame
    void Update()
    {
        if (eventCount == 0) transform.FindChild("Event Activate Trigger").gameObject.SetActive(false);

        if (soundActive && !eventSounds.isPlaying)
        {
            eventSounds.Play();
            soundActive = false;
            eventCount--;
        }

    }

    public void ActivateEvent()
    {
        soundActive = true;
    }
}

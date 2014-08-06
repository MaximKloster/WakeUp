using UnityEngine;
using System.Collections;

public class PatientControler : MonoBehaviour {
    [SerializeField]
    AudioClip attack = null;

    AudioSource patientSounds;
    Animation animation;
	// Use this for initialization
	void Start () {
        patientSounds = gameObject.AddComponent<AudioSource>();
        patientSounds.playOnAwake = false;
        animation = transform.GetChild(0).GetComponentInChildren<Animation>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
          
            animation.Play("attack");
            PlaySoundAttack();

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animation.Play("walk");
           
        }
    }









    void PlaySoundAttack()
    {
        if (attack)
        {
            patientSounds.clip = attack;
            patientSounds.Play();
        }
    }
}

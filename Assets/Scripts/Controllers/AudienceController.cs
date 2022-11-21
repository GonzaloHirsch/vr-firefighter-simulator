using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceController : MonoBehaviour
{
    public GameObject[] puppets;
    public Transform player;
    public RuntimeAnimatorController celebrationAnimator;
    public AudioClip celebrationSoundClip;
    private bool celebreating = false;

    private void Update()
    {
        if(celebreating)
        {
            for (int i = 0; i < puppets.Length; i++)
            {
                Quaternion targetRotation = Quaternion.LookRotation(player.position - puppets[i].transform.position);
                puppets[i].transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Min(0.5f * Time.deltaTime, 1));
            }
        }
    }

    public void Celebrate()
    {
        for (int i=0; i < puppets.Length; i++)
        {
            Animator anim = puppets[i].GetComponent<Animator>();
            AudioSource audio = puppets[i].GetComponent<AudioSource>();
            Quaternion targetRotation = Quaternion.LookRotation(player.position - puppets[i].transform.position);

            anim.runtimeAnimatorController = celebrationAnimator;
            audio.Stop();
            audio.clip = celebrationSoundClip;
            audio.Play();
            //puppets[i].transform.rotation = targetRotation;
        }
    }
}

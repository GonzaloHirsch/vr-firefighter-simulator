using System.Collections;
using UnityEngine;

public class AudienceController : MonoBehaviour
{
    public GameObject[] puppets;
    public AudioSource[] crowdAudioSources;
    public Transform player;
    public RuntimeAnimatorController celebrationAnimator;
    public AudioClip celebrationSoundClip;
    public AudioClip crowdCelebrationSoundClip;
    private bool celebrating = false;
    public FadeController fadeController;

    private void Start()
    {
        celebrating = false;
    }

    private void Update()
    {
        if(celebrating)
        {
            for (int i = 0; i < puppets.Length; i++)
            {
                Quaternion rot = Quaternion.LookRotation(player.position - puppets[i].transform.position);
                puppets[i].transform.rotation = Quaternion.Slerp(puppets[i].transform.rotation, rot, Time.deltaTime);
            }
        }
    }

    public void StartVictory()
    {
        StartCoroutine(StartVictorySequence());
    }

    private IEnumerator StartVictorySequence()
    {
        Celebrate();
        yield return new WaitForSeconds(10f);
        fadeController.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneController.GoToMenuScene();
    }

    public void Celebrate()
    {
        celebrating = true;
        for (int i=0; i < puppets.Length; i++)
        {
            Animator anim = puppets[i].GetComponent<Animator>();
            AudioSource audio = puppets[i].GetComponent<AudioSource>();
            Quaternion targetRotation = Quaternion.LookRotation(player.position - puppets[i].transform.position);

            anim.runtimeAnimatorController = celebrationAnimator;
            audio.Stop();
            audio.clip = celebrationSoundClip;
            audio.loop = true;
            audio.volume = 0.8f;
            audio.Play();
        }
        for (int i = 0; i < crowdAudioSources.Length; i++)
        {
            AudioSource audio = crowdAudioSources[i];
            audio.Stop();
            audio.clip = crowdCelebrationSoundClip;
            audio.loop = true;
            audio.volume = 0.7f;
            audio.Play();
        }
    }
}

using UnityEngine;
using System.Collections;

public class FireAlarm : MonoBehaviour {
    public FadeController fadeController;
    private AudioSource audioSource;

    public void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void OnPointerClick() {
        StartCoroutine(OnPointerClickCoroutine());
    }

    IEnumerator OnPointerClickCoroutine()
    {
        this.audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        fadeController.FadeOut();
        for (int i = 0; i < 6; i++)
        {
            this.audioSource.volume = this.audioSource.volume * 0.7f;
            yield return new WaitForSeconds(0.25f);
        }
        SceneController.GoToGameScene();
    }

    public void OnPointerEnter() {
        NarratorController.Instance.NarrateAlarm();
    }
}
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
    }

    // Update is called once per frame
    public void FadeOut()
    {
        animator.Play("FadeOut");
    }

    public void FadeIn()
    {
        animator.Play("FadeIn");
    }
}

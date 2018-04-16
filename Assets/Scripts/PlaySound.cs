using UnityEngine;

public class PlaySound : StateMachineBehaviour
{
    public AudioClip[] sounds;
    public bool loop;
    public float delay; //Delai avant le prochain son
    public float delayTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.GetComponent<AudioSource>();
        var n = Random.Range(0, sounds.Length);
        audioSource.clip = sounds[n];
        audioSource.enabled = true;
        audioSource.Play();
        delayTimer = delay;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (loop)
        {
            delayTimer -= Time.deltaTime;

            AudioSource audioSource = animator.GetComponent<AudioSource>();
            if (!audioSource.isPlaying && delayTimer <= 0f)
            {
                var n = Random.Range(0, sounds.Length);
                audioSource.clip = sounds[n];
                audioSource.enabled = true;
                audioSource.Play();
                delayTimer = delay;
            }
        }
    }
}
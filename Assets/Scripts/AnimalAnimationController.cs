using UnityEngine;

public class AnimalAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;
    public string idleAnimationName = "Idle";
    public string walkAnimationName = "Walk";
    
    [Header("Auto Animation")]
    public bool playIdleOnStart = true;
    public bool randomIdleAnimations = true;
    public float minIdleInterval = 3f;
    public float maxIdleInterval = 8f;
    
    private float nextIdleTime;
    
    private void Start()
    {
        // Auto-find animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
        
        if (animator != null && playIdleOnStart)
        {
            PlayIdle();
        }
        
        if (randomIdleAnimations)
        {
            nextIdleTime = Time.time + Random.Range(minIdleInterval, maxIdleInterval);
        }
    }
    
    private void Update()
    {
        if (randomIdleAnimations && animator != null && Time.time >= nextIdleTime)
        {
            PlayRandomIdleAnimation();
            nextIdleTime = Time.time + Random.Range(minIdleInterval, maxIdleInterval);
        }
    }
    
    public void PlayIdle()
    {
        if (animator != null && !string.IsNullOrEmpty(idleAnimationName))
        {
            animator.Play(idleAnimationName);
        }
    }
    
    public void PlayWalk()
    {
        if (animator != null && !string.IsNullOrEmpty(walkAnimationName))
        {
            animator.Play(walkAnimationName);
        }
    }
    
    public void PlayAnimation(string animationName)
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
    }
    
    private void PlayRandomIdleAnimation()
    {
        if (animator == null) return;
        
        // Get all animation clips
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        if (clips.Length > 0)
        {
            // Filter idle animations (you can customize this logic)
            System.Collections.Generic.List<AnimationClip> idleClips = new System.Collections.Generic.List<AnimationClip>();
            
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.ToLower().Contains("idle"))
                {
                    idleClips.Add(clip);
                }
            }
            
            if (idleClips.Count > 0)
            {
                int randomIndex = Random.Range(0, idleClips.Count);
                animator.Play(idleClips[randomIndex].name);
            }
        }
    }
    
    public void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.speed = speed;
        }
    }
}

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class ExternalAnimationController : MonoBehaviour
{
    private PlayableGraph playableGraph;
    private AnimationPlayableOutput animationOutput;
    private AnimationClipPlayable clipPlayable;

    private Animator animator;
    private bool existsPlayableGraph;
    public void PlayExternalAnimation(AnimationClip clip)
    {
        if (playableGraph.IsValid())
            playableGraph.Destroy();

        existsPlayableGraph = true;
        animator.SetBool("PlayActivityAnimation", true);
        playableGraph = PlayableGraph.Create("ExternalAnimationGraph");
        animationOutput = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", GetComponent<Animator>());
        clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        animationOutput.SetSourcePlayable(clipPlayable);
        playableGraph.Play();
    }

    public void ManuallyDestroyAnimation()
    {
        if (existsPlayableGraph)
            playableGraph.Destroy();

        animator.SetBool("PlayActivityAnimation", false);
        existsPlayableGraph = false;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (playableGraph.IsValid() && clipPlayable.IsDone())
        {
            ManuallyDestroyAnimation();
        }
    }
}

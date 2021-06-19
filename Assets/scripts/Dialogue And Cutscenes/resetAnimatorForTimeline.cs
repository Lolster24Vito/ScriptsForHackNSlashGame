using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class resetAnimatorForTimeline : MonoBehaviour
{
    public Animator playerAnimator;
    public RuntimeAnimatorController playerAnim;
    public PlayableDirector director;
    bool fix = true;
    bool check = true;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
    }

    private void Update()
    {

        

        if (director.state != PlayState.Playing &&!fix)
        {
            fix = true;
            playerAnimator.runtimeAnimatorController = null;
            playerAnimator.runtimeAnimatorController = playerAnim;
		director.Stop();
            Debug.Log("TESTTTIIIINNNGGG");
        }
        if (director.state == PlayState.Playing&&fix)
        {
            fix = false;
            playerAnimator.runtimeAnimatorController=playerAnim;

        Debug.Log("Testing");

        }

    }
    public void fixAnimatorSoThatObjectCanMove() {
        playerAnimator.runtimeAnimatorController = playerAnim;
    }

}

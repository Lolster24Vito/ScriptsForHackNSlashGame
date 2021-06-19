using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;
using Cinemachine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;


 
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject pressEText;

    public GameObject dialogueUIParent;
    public PlayableDirector playableDirector;
    private bool typing = false;
    private bool waitForTimelineBool = false;
    public static bool isInDialogue = false;
    public AudioClip[] textSound;
    public float waitTimeForLetters=0.05f;

    private Transform player;


    private int audioClipCounter=0;
    private AudioSource audioSource;

    private Animator helperAnimator;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
         //   DontDestroyOnLoad(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;


    }
    private void Update()
    {

    }
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("OnEnable2 called");
    }
    public void ZoomInCamera(CinemachineVirtualCamera camera)
    {
        camera.m_Lens.OrthographicSize=3f;
}
    public void ZoomToNormalCamera(CinemachineVirtualCamera camera)
    {
        camera.m_Lens.OrthographicSize = 7.43f;
    }

    public void UIActive(bool value)
    {
        dialogueUIParent.SetActive(value);
    }
   public void playerMovementActive(bool value)
    {
        player.GetComponent<charecterMovementController>().enabled = value;
        player.GetComponent<CharecterAttackController>().enabled = value;
     //   player.GetComponent<AbilityController>().enabled = value;


    }
    public void WriteText(Line[] line)
    {
        playerMovementActive(false);
        StartCoroutine(waitForButtonPress(line));

    }
    public void WriteText(LinesForEvents line)
    {
        StopAllCoroutines();
        playerMovementActive(false);
        StartCoroutine(waitForButtonPressButInTimeline(line.lines));

    }



    IEnumerator waitForButtonPress(Line[]line)
    {
        dialogueText.text = "";
        isInDialogue = true;
        UIActive(true);
        int counter =0;
        while (counter <= line.Length)
        {
            if (Input.GetKeyUp(KeyCode.E) && !typing)
            {
                if (!waitForTimelineBool || (waitForTimelineBool && playableDirector.state != PlayState.Playing))
                {
                    if (counter != line.Length)
                    {
                        waitForTimelineBool = false;
                        playableDirector.Stop();
                        StartCoroutine(setText(line[counter]));
                    }
                    counter++;



                }
            }


            yield return null;
        }
        UIActive(false);
        playerMovementActive(true);
        isInDialogue = false;

    }







    IEnumerator setText(Line line)
    {
        pressEText.active = false;
        typing = true;
        nameText.text = line.speaker;
        dialogueText.text = "";
        if (line.kk != null)
        {
            line.kk.Invoke();
        }
        foreach (char letter in line.Text.ToCharArray())
        {
            dialogueText.text += letter;
            if (textSound != null)
            {
                if (audioClipCounter < textSound.Length-1)
                    audioClipCounter++;
                else
                {
                    audioClipCounter = 0;
                }
                audioSource.PlayOneShot(textSound[audioClipCounter]);

               
            }

            yield return new WaitForSeconds(waitTimeForLetters);
        }
        pressEText.active = true;
        typing = false;
    }



    void SetText(Line line)
    {
        dialogueText.text = line.Text;
        nameText.text = line.speaker;

        if (line.kk != null)
        {
            line.kk.Invoke();
        }

    }

    public void playPlayableDirectorThatCanBeSkipped(TimelineAsset timelineAsset)
    {
        playableDirector.Play(timelineAsset);
    }
    public void playPlayableDirectorThatHasToFinish(TimelineAsset timelineAsset)
    {
        waitForTimelineBool = true;
        playableDirector.Play(timelineAsset);

    }



    IEnumerator waitForButtonPressButInTimeline(Line[] line)
    {
        playableDirector.Pause();
        isInDialogue = true;
        UIActive(true);
        int counter = 0;
        StartCoroutine(setText(line[counter]));
         counter ++;

        while (counter <= line.Length)
        {
            if (Input.GetKeyUp(KeyCode.E) && !typing)
            {

                if (counter <= line.Length)
                {
                    waitForTimelineBool = false;
                    if (counter == line.Length)
                    {
                        break;
                    }
                    StartCoroutine(setText(line[counter]));
                    counter++;
                }



            }


           
            yield return null;
        }

        UIActive(false);
        playerMovementActive(false);
        isInDialogue = false;
        playableDirector.Play();

    }

   





    /*

    IEnumerator waitForButtonPressButInTimeline(string line)
    {
        playableDirector.Pause();
        isInDialogue = true;
        UIActive(true);
        int counter = 0;

        StartCoroutine(setTextInTimeline(line));
        while (counter < 1)
        {
            //Write automatically the line and then exit out of the dialogue when E is pressed
            if (Input.GetKeyUp(KeyCode.E) && !typing)
            {
                if (!waitForTimelineBool || (waitForTimelineBool && playableDirector.state != PlayState.Playing))
                {
                    if (counter != line.Length)
                    {
                        waitForTimelineBool = false;
                    }
                    counter++;
		timer=0;



                }
            }

}
	
            yield return null;
        }
        UIActive(false);
        playerMovementActive(false);
        isInDialogue = false;
        playableDirector.Play();


    }







    IEnumerator setTextInTimeline(Line line)
    {
        typing = true;
        nameText.text = line.speaker;
        dialogueText.text = "";
        if (line.kk != null)
        {
            line.kk.Invoke();
        }
        foreach (char letter in line.Text.ToCharArray())
        {
            dialogueText.text += letter;
            Debug.Log("1l");
            if (textSound != null)
            {
                if (audioClipCounter < textSound.Length - 1)
                    audioClipCounter++;
                else
                {
                    audioClipCounter = 0;
                }
                Debug.Log("2l");
                audioSource.PlayOneShot(textSound[audioClipCounter]);


            }

            yield return new WaitForSeconds(waitTimeForLetters);
        }
        typing = false;
    }

    IEnumerator setTextInTimeline(string line)
    {
        typing = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            if (textSound != null)
            {
                if (audioClipCounter < textSound.Length - 1)
                    audioClipCounter++;
                else
                {
                    audioClipCounter = 0;
                }
                Debug.Log("2l");
                audioSource.PlayOneShot(textSound[audioClipCounter]);


            }
            yield return new WaitForSeconds(waitTimeForLetters);
        }
        typing = false;
    }


    */

	public void destroyGameObject(GameObject gameObject){
	Destroy(gameObject);
}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneLoadManager : MonoBehaviour
{

    [SerializeField] UnityEvent eventsOnLoadToLevel;
    public Image fadeInOutImage;

   

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        //SceneManager.sceneLoaded += OnSceneLoaded;
        if (eventsOnLoadToLevel != null)
        {
            eventsOnLoadToLevel.Invoke();
        }
        Fade_Out();
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (eventsOnLoadToLevel != null)
        {
            eventsOnLoadToLevel.Invoke();
        }
        Fade_Out();
    }

  public void LoadScene(int sceneIndex)
    {
        StartCoroutine(FadeIn(sceneIndex));
        
    }
     IEnumerator FadeIn()
    {
        Color color = fadeInOutImage.color;
        while (fadeInOutImage.color.a < 0.99)
        {
            fadeInOutImage.color=new Color(color.r,color.g,color.b, 
                Mathf.MoveTowards(fadeInOutImage.color.a, 255f, 0.85f * Time.deltaTime));
            yield return null;
        }
        
    }
    IEnumerator FadeIn(int sceneIndex)
    {
        fadeInOutImage.color = new Color(0, 0, 0, 0);
        float time = 0.7f;
        float i = 0;
        float alpha = 0;

        while (i < 1)
        {
            i += Time.deltaTime / time;
            alpha = Mathf.Lerp(0, 1.0f, i);

            fadeInOutImage.color = new Color(0, 0, 0, alpha);

            yield return 0;
        }

        SceneManager.LoadScene(sceneIndex);
    }
    IEnumerator FadeOut()
    {
        fadeInOutImage.color = new Color(0, 0, 0, 255);
        float time = 0.7f;
            float i = 1;
            float alpha = 0;

            while (i > 0)
            {
                i -= Time.deltaTime / time;
                alpha = Mathf.Lerp(0, 1.2f, i);

            fadeInOutImage.color = new Color(0, 0, 0,alpha);
            yield return 0;
            }
        
    }


    public void Fade_In()
    {
        StartCoroutine(FadeIn());
    }
    public void Fade_Out()
    {
        StartCoroutine(FadeOut());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

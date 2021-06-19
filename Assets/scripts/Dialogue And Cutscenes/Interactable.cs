using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public Line[] speak;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)&&!MessageManager.isInDialogue)
            {
                MessageManager.Instance.WriteText(speak);
            }
        }
    }

public void SayYourLine(){
                MessageManager.Instance.WriteText(speak);
}

}

[System.Serializable]
public struct Line
{
    public string Text;
    public string speaker;
    public UnityEvent kk;
    public Sprite face;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    [SerializeField]private int sortingOrderBase = 5000;
    [SerializeField]private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;

    private Renderer myRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }
}

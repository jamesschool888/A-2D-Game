using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showOrHideControls : MonoBehaviour
{
    public GameObject show;
    public GameObject hide;

    bool controlsShown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            showControls();
        else
            deactivateShowControls();
    }

    void deactivateShowControls()
    {
        if (controlsShown)
        {
            controlsShown = false;
            hide.SetActive(false);
            show.SetActive(true);
        }
    }

    void showControls()
    {
        if (!controlsShown)
        {
            controlsShown = true;
            show.SetActive(false);
            hide.SetActive(true);
        }
        
    }
}

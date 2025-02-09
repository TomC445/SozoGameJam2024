using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayWithinBounds : MonoBehaviour
{
    public float xBound;
    public float zBound;
    public float outerXBound;
    public float outerZBound;
    public float innerXBound;
    public float innerZBound;

    public GameObject panel;
    public GameObject minimapCover;

    private Image panelImage;
    private Image minimapCoverImage;

    void Start()
    {
        panelImage = panel.GetComponent<Image>();
        minimapCoverImage = minimapCover.GetComponent<Image>();
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > innerXBound || Mathf.Abs(transform.position.z) > innerZBound)
        {
            float xproximity = (Mathf.Abs(transform.position.x)-innerXBound) / (outerXBound-innerXBound);
            float zproximity = (Mathf.Abs(transform.position.z)-innerZBound) / (outerZBound-innerZBound);
            float proximity = Mathf.Max(xproximity, zproximity);
            if (panelImage)
            {
                panelImage.color = new Color(1, 1, 1, proximity);
            }

            if (minimapCover)
            {
                minimapCoverImage.color = new Color(1, 1, 1, proximity);
            }
        }

        if(transform.position.x > xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        else if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
    }
}

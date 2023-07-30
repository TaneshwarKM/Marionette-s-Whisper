

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickupLetter : MonoBehaviour
{
    public static pickupLetter instance;
    public GameObject collectTextObj, intText;
    public AudioSource pickupSound, ambianceLayer1, ambianceLayer2, ambianceLayer3, ambianceLayer4;
    public bool interactable;
    public static int pagesCollected;
    public Text collectText;
    

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);
            interactable = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }
    void Update()
    {
        if (interactable == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                pagesCollected = pagesCollected + 1;
                collectText.text = pagesCollected + "/4 pages";
                collectTextObj.SetActive(true);
                pickupSound.Play();
                if (pagesCollected == 1)
                {
                    ambianceLayer1.Play();
                }
                if (pagesCollected == 2)
                {
                    ambianceLayer2.Play();
                }
                if (pagesCollected == 3)
                {
                    ambianceLayer3.Play();
                }
                if (pagesCollected == 4)
                {
                    ambianceLayer4.Play();
                }
                
                intText.SetActive(false);
                this.gameObject.SetActive(false);
                interactable = false;
            }
        }
    }
}

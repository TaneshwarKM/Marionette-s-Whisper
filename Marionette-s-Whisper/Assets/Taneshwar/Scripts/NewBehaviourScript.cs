using UnityEngine;

public class PageCollector : MonoBehaviour
{
    private pickupLetter pagescollected;
    private int pagesCollected = 0;

    public int PagesCollected => pagesCollected;

    public void CollectPage()
    {
        pagesCollected++;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [SerializeField] private Image[] stars; 

    private int currentStarIndex = 0; 

    private void Start()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }

    public void DisableNextStar()
    {
        if (currentStarIndex < stars.Length)
        {
            stars[currentStarIndex].gameObject.SetActive(false);
            currentStarIndex++;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DisableButtonOnClicks : MonoBehaviour
{
    [SerializeField] private Button myButton;
    [SerializeField] private int maxClicks = 4; 

    private int clickCount = 0;

    private void Start()
    {
        myButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        clickCount++;

        if (clickCount >= maxClicks)
        {
            myButton.interactable = false; 
        }
    }
}

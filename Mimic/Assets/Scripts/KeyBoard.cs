using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> keyboards = new List<GameObject>();

    private void Start()
    {
        DisableKeyboard();
    }

    public void OnTextClicked(string textboxName)
    {
        
        foreach (GameObject keyboard in keyboards)
        {
            Debug.Log("´Ù¼û±è");
            keyboard.SetActive(false);
        }

        foreach (GameObject keyboard in keyboards)
        {
            Debug.Log("KeyBoard: " + keyboard.name); 
            if (keyboard.name == textboxName + "Keyboard")
            {
                Debug.Log("textbox name :" + textboxName);
                keyboard.SetActive(true);
                break; 
            }
        }
    }
    public void DisableKeyboard()
    {
        foreach (GameObject keyboard in keyboards)
        {
                keyboard.SetActive(false);
        }
    }
}

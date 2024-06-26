using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> keyboards = new List<GameObject>();

    private void Start()
    {
        
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }
    }

    public void OnTextClicked(string textboxName)
    {
        
        foreach (GameObject keyboard in keyboards)
        {
            Debug.Log("�ټ���");
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
}

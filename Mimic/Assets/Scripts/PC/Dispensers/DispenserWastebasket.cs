using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserWastebasket : MonoBehaviour, IDispenser
{
    public IEnumerator GenerateFood(GameObject _food)
    {
        yield break;
    }

    public bool GetIsGenerate()
    {
        return false;
    }

    public void OperateDispenser(GameObject _player)
    {
        Debug.Log("¹ö¸®±â");
        Destroy(_player.GetComponentInChildren<BindFood>().Food);
    }

}

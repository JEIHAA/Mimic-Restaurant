using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRootParent : MonoBehaviour
{
    public static GameObject GetTopmostParent(GameObject child)
    {
        if (child == null)
        {
            return null;
        }

        Transform currentParent = child.transform;

        while (currentParent.parent != null)
        {
            currentParent = currentParent.parent;
        }
       return currentParent.gameObject;
    }
}

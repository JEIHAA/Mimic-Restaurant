using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform m_muzzle;
    public GameObject m_shotPrefab;

    public void Shoot()
    {
        GameObject go = GameObject.Instantiate(m_shotPrefab, m_muzzle.position, m_muzzle.rotation) as GameObject;
        GameObject.Destroy(go, 2f);
    }
}
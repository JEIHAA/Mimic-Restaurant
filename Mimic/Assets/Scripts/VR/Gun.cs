using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public int poolSize = 30; 
    public float bulletSpeed = 10f; 
    public float bulletLifetime = 2f; 

    private List<GameObject> bulletPool; 
    private void Start()
    {
        // �Ѿ� Ǯ �ʱ�ȭ
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = GetBulletFromPool();

        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            

            bullet.SetActive(true);

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = transform.forward * bulletSpeed;
            }

            StartCoroutine(DisableBulletAfterLifetime(bullet));
        }
    }


    IEnumerator DisableBulletAfterLifetime(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);

        if (bullet.activeSelf) 
        {
            bullet.SetActive(false);
        }
    }

    GameObject GetBulletFromPool()
    {
        // ��Ȱ��ȭ�� �Ѿ��� ã�� ��ȯ
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }
}

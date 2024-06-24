using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject muzzle;
    public GameObject bulletPrefab;
    public int poolSize = 20;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;

    private List<GameObject> bulletPool;

    private void Start()
    {
        bulletPool = new List<GameObject>();
        InitializeBulletPool();
    }
    private void Update()
    {
        CheckBulletLifetime();
    }

    private void InitializeBulletPool()
    {
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
            bullet.transform.position = muzzle.transform.position;
            bullet.transform.rotation = muzzle.transform.rotation;

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
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        return null;
    }

    void CheckBulletLifetime()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (bullet.activeSelf)
            {
                float timeSinceActivated = Time.time - bullet.GetComponent<Bullet>().activationTime;
                if (timeSinceActivated > bulletLifetime)
                {
                    bullet.SetActive(false);
                }
            }
        }
    }


}

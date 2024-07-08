using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject muzzle;
    public GameObject bulletPrefab;
    public int poolSize = 20;
    [SerializeField] private BulletStat bulletData = null;
    public float bulletLifetime = 3f;

    private List<GameObject> bulletPool;
    public VRPlayer vrPlayer;

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
        GameObject bulletManager = GameObject.Find("BulletManager");
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletManager.transform);
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
                bulletRigidbody.velocity = transform.forward * bulletData.bulletSpeed;
            }

            StartCoroutine(DisableBulletAfterLifetime(bullet));

            vrPlayer.DecreaseHungry(5); 
        }
    }


    private IEnumerator DisableBulletAfterLifetime(GameObject bullet)
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

    private void CheckBulletLifetime()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // �Ѿ� ������
    public int poolSize = 30; // ������Ʈ Ǯ ũ��
    public float bulletSpeed = 10f; // �Ѿ� �߻� �ӵ�
    public float bulletLifetime = 2f; // �Ѿ� ���� �ð� (��)

    private List<GameObject> bulletPool; // �Ѿ� ������Ʈ Ǯ

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
        // �Ѿ� ������Ʈ Ǯ���� ��� ������ �Ѿ� ã��
        GameObject bullet = GetBulletFromPool();

        if (bullet != null)
        {
            // �Ѿ� �߻� ��ġ�� ���� ����
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            // �Ѿ��� Ȱ��ȭ�Ͽ� �߻�
            bullet.SetActive(true);

            // �Ѿ˿� Rigidbody ������Ʈ�� ����Ͽ� �߻� �ӵ� ����
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

        if (bullet.activeSelf) // activeSelf�� ����Ͽ� �θ� Ȱ�� ���οʹ� ������� �Ѿ��� Ȱ�� ���¸� Ȯ���մϴ�.
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

        // ��� ������ �Ѿ��� ������ null ��ȯ
        return null;
    }
}

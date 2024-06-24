using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public int poolSize = 30; // 오브젝트 풀 크기
    public float bulletSpeed = 10f; // 총알 발사 속도
    public float bulletLifetime = 2f; // 총알 생존 시간 (초)

    private List<GameObject> bulletPool; // 총알 오브젝트 풀

    private void Start()
    {
        // 총알 풀 초기화
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
        // 총알 오브젝트 풀에서 사용 가능한 총알 찾기
        GameObject bullet = GetBulletFromPool();

        if (bullet != null)
        {
            // 총알 발사 위치와 방향 설정
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            // 총알을 활성화하여 발사
            bullet.SetActive(true);

            // 총알에 Rigidbody 컴포넌트를 사용하여 발사 속도 적용
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

        if (bullet.activeSelf) // activeSelf를 사용하여 부모 활성 여부와는 관계없이 총알의 활성 상태를 확인합니다.
        {
            bullet.SetActive(false);
        }
    }

    GameObject GetBulletFromPool()
    {
        // 비활성화된 총알을 찾아 반환
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // 사용 가능한 총알이 없으면 null 반환
        return null;
    }
}

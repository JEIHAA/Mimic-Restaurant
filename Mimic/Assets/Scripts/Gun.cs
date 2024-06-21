using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;        
    public GameObject shotPrefab;
    public float shotForce = 15f;
    [SerializeField] private int gunDamage = 100;

    // 발사 메서드

    public void Shoot()
    {
        GameObject shot = Instantiate(shotPrefab, muzzle.position, muzzle.rotation);

        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(muzzle.forward * shotForce, ForceMode.Impulse);
        }

        Destroy(shot, 2f);
    }

}

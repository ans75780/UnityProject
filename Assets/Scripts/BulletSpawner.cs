using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    private static BulletSpawner instance;
    public static BulletSpawner Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float radius = 15f;
    public int segments = 360; // 원을 구성할 선분의 개수
    public Color color = Color.green;
    
    public GameObject bulletPrefab;
    
    public float bulletSpeed = 15;
    
    
    IObjectPool<Bullet> bulletPool;

    void Awake()
    {
        BulletSpawner.instance = this;

        bulletPool = new ObjectPool<Bullet>
            (createFunc: CreateBullet);

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();


        return bullet;
    }
    
    
    
    
    public void SpawnBullet(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, 360);
            Vector3 pos = new Vector3(
                
                Mathf.Cos(rand* Mathf.Deg2Rad) * radius,
                1,
                Mathf.Sin(rand * Mathf.Deg2Rad) * radius
                );
            
            Bullet bullet = bulletPool.Get();
            
            bullet.transform.position = pos;
            if (bullet != null)
            {
                bullet.gameObject.SetActive(true);
                
                pos.y = 0;
                bullet.InitBullet(-pos.normalized, bulletSpeed);
                bullet.transform.SetParent(transform, false);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (segments < 1) return;

        float angleIncrement = (360f / segments) * Mathf.Deg2Rad; 
        
        for (int i = 0; i < segments; i++)
        {
            Vector3 p1 = new Vector3(radius * Mathf.Cos(i * angleIncrement), 0, radius * Mathf.Sin(i * angleIncrement));
            Vector3 p2 = new Vector3(radius * Mathf.Cos((i + 1) * angleIncrement), 0, radius * Mathf.Sin((i + 1) * angleIncrement));
            Gizmos.DrawLine(p1, p2);
        }
    }
    public void ReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Release(bullet);
        
    }
}

using System.Text;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 dir;
    private float speed;

    public float lifeTime = 8;
    
    public void InitBullet(Vector3 _dir, float _speed)
    {
        dir = _dir;
        speed = _speed;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.transform.Translate(speed * Time.deltaTime * dir, Space.World); 
        
    }

    void OnEnable()
    {
        Invoke(nameof(ReleaseBullet), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
    
    void ReleaseBullet()
    {
        BulletSpawner.Instance.ReleaseBullet(this);
    }
    
    void OnTriggerEnter(Collider other)
    {
        ////태그가 다를 경우
        //if (other.CompareTag("Player"))
        //{
        //    Damageable damageable = other.GetComponent<Damageable>();
        //    if (damageable != null)
        //    {
        //        damageable.ApplyDamage(1);
        //        
        //        //여기에 불렛 비활성화 코드 삽입 임시로 삭제처리하겠음
        //        ReleaseBullet();
        //    }
        //}
    }
}

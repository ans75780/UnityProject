    using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;


public class VisionSensor : MonoBehaviour, ISenseModality
{
    public StimulusType Type {
        get
        {
            return StimulusType.Vision;
        }
    }

    public float viewRadius = 15f;

    [Range(0.01f, 179f)] 
    public float fov = 90;
    
    private float cosHalfFov;
    
    void Awake()
    {
        //미리 각도 계산
        cosHalfFov = Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
    }
    
    public bool TrySense(SenseContext context, List<SenseHit> hits)
    {
        int count = Physics.OverlapSphereNonAlloc
        (
            context.owner.position, 
            viewRadius, 
            context.overlapCache, 
            context.targetMask, 
            QueryTriggerInteraction.Ignore
        );
        bool sensedSomething = false;

        
        Vector3 forward = context.owner.forward;

        for (int i = 0; i < count; i++)
        {
            
            Collider collider = context.overlapCache[i];

            if (collider == null) continue;
            if (collider.transform == context.owner.transform) continue;
            
            Vector3 colliderToOwner = collider.transform.position - context.owner.position;
            
            float distance = colliderToOwner.magnitude;

            // 충돌 대상과의 거리 차이가 0이라면
            if (distance < Mathf.Epsilon) continue;
            
            float dot  = Vector3.Dot(forward, colliderToOwner.normalized);

            //감지 시야 바깥에 있다면 감지X
            if (dot < cosHalfFov) continue;

            //센서와 대상 사이 장애물이 있는지 검사.
            if (Physics.Linecast(context.owner.position, 
                    collider.transform.position, 
                    context.occluderMask, 
                    QueryTriggerInteraction.Ignore))
            {
                continue;
            }
            SenseHit hit = new SenseHit();
            hit.target = collider.gameObject;
            hit.type = StimulusType.Vision;
            hit.weight = dot; //정면에 가까울수록 가중치 높음
            hit.lastKnownPos = collider.transform.position;
            hit.time = context.now;

            sensedSomething = true;
            hits.Add(hit);
            
        }
        return sensedSomething;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmosSelected()
    {
        // 탐색 반경 (구체)
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, viewRadius);

        // 시야 방향
        Vector3 forward = transform.forward;

        // 시야각 절반
        float halfFov = fov * 0.5f;

        // 좌우 방향 벡터
        Quaternion leftRot = Quaternion.Euler(0, -halfFov, 0);
        Quaternion rightRot = Quaternion.Euler(0, halfFov, 0);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftDir * viewRadius);
        Gizmos.DrawRay(transform.position, rightDir * viewRadius);

        // 정면 방향
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward * viewRadius);
    }
    
}
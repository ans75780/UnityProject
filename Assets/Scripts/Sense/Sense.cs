using System.Collections.Generic;
using UnityEngine;



public enum StimulusType { Vision, Hearing, End }

//감지에 걸린 상황에 대한 정보
public struct SenseHit
{
    public GameObject target;
    public StimulusType type;
    public float weight;
    public Vector3 lastKnownPos;
    public float time;
}

//감지에 대한 정보)
public struct SenseContext
{
    public Transform owner;
    public float now;
    public LayerMask targetMask;
    public LayerMask occluderMask;
    public Collider[] overlapCache;
    public RaycastHit[] raycastCache;
}

public interface ISenseModality
{
    StimulusType Type { get; }
    bool TrySense(SenseContext context, List<SenseHit> hits );
}


public class Sense : MonoBehaviour
{
    public float tickInterval = 0.1f;
    public float jitterRatio = 0.25f;
    public int maxTracked = 16;
    
    
    public LayerMask targetMask;
    
    public LayerMask occluderMask;
    public int overlapCacheSize = 128;
    public int raycastCacheSize = 64;
    
    
    
    private List<ISenseModality> modalities = new List<ISenseModality>();
    private Dictionary<GameObject, SenseHit> memory = new Dictionary<GameObject, SenseHit>();
    private SenseContext context;
    
    private List<SenseHit> tickHits = new List<SenseHit>();
    
    private List<GameObject> toRemove = new List<GameObject>();
    
    private float nextTick;

    
    public delegate void SenseEventHandler(SenseHit hit);
    
    public event SenseEventHandler OnSensed;
    public event SenseEventHandler OnLost;
    
    
    public event SenseEventHandler OnVisionSensed;
    public event SenseEventHandler OnVisionLost;

    //Hearing은 추후 구현
    
    
    
    void Awake()
    {
        context = new SenseContext();
        
        context.owner = transform;
        context.occluderMask = occluderMask;
        context.targetMask = targetMask;
        context.overlapCache = new Collider[overlapCacheSize];
        context.raycastCache = new RaycastHit[raycastCacheSize];
        
        nextTick = Time.time + tickInterval * UnityEngine.Random.Range(0f, jitterRatio);
    }

    public void AddModality(ISenseModality modality)
    {
        if (modality == null || modalities.Contains(modality))
            return;
        
        modalities.Add(modality);
    }

    public void RemoveModality(ISenseModality modality)
    {
        if (modality == null)
            return;
        
        modalities.Remove(modality);
    }
    
    void Update()
    {
        float now = Time.time;
        if (now < nextTick) return;
        
        nextTick = now + tickInterval;
        context.now = now;
        
        Tick();
    }
    //매 프레임마다 검사하는건 비용적으로 커서, 자체적인 tick을 줘서 관리함.
    void Tick()
    {
        //저번 프레임에 검출된 대상을 제거
        tickHits.Clear();
        toRemove.Clear();
        
        for (int i = 0; i < modalities.Count; i++)
        {
            modalities[i].TrySense(context, tickHits);
        }

        for (int i = 0; i < tickHits.Count; i++)
        {
            SenseHit hit = tickHits[i];

            if (hit.target == null)
                continue;
            
            bool isNew = !memory.ContainsKey(hit.target);
            memory[hit.target] = hit;

            //저번 틱에 없었고, 현재 틱에 들어온 타겟이라면
            if (isNew)
            {
                InvokeIfNotNull(OnSensed, hit);
                if (hit.type == StimulusType.Vision)
                {
                    InvokeIfNotNull(OnVisionSensed, hit);
                }
            }
        }

        foreach (KeyValuePair<GameObject, SenseHit> pair in memory)
        {
            bool stillThere = false;

            for (int i = 0; i < tickHits.Count; i++)
            {
                if (tickHits[i].target == pair.Key)
                {
                    stillThere = true;
                    break;
                }
            }
            if (stillThere == false)
            {
                toRemove.Add(pair.Key);
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            GameObject key = toRemove[i];
            SenseHit hit = memory[key];
            
            memory.Remove(key);
            InvokeIfNotNull(OnLost, hit);

            if (hit.type == StimulusType.Vision)
            {
                InvokeIfNotNull(OnVisionLost, hit);
            }
            //else//hearing 구현 예정
            //{
            //    
            //}
        }
    }

    private void InvokeIfNotNull(SenseEventHandler e, SenseHit hit)
    {
        if (e != null) e.Invoke(hit);
    }
}

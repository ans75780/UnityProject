using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyData", menuName = "Scriptable Objects/SO_EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    [SerializeField] 
    private float maxHp;

    public float MaxHp
    {
        get { return maxHp; }
    }
    
    [SerializeField] 
    private float atk;

    public float Atk
    {
        get { return atk; }
    }

    [SerializeField] 
    private float attackRange;

    public float AttackRange
    {
        get
        {
            return attackRange;
        }
    }
    
    [SerializeField] 
    private float moveSpeed;

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
    
}



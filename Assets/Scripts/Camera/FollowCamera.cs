using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    
    void Update()
    {
        this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }
    
    
    
}

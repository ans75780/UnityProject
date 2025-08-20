using UnityEngine;

public class Enviroment_Sun : MonoBehaviour
{
    public float rotateSpeed = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(rotateSpeed * Time.deltaTime, 0, 0)) ;
    }
}

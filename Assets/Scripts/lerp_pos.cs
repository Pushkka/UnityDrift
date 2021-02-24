using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerp_pos : MonoBehaviour
{
    public GameObject target;
    public float modifier;
    public bool Rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 0.05)
            transform.position = Vector3.Lerp(transform.position,target.transform.position, Time.deltaTime * modifier);
        if (Rotation)
            transform.rotation = Quaternion.Lerp(transform.rotation,target.transform.rotation, Time.deltaTime * modifier/2);
    }
}

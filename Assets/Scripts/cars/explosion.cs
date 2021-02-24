using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public float force;
    private float timer;
    public float zaderjka;
    public GameObject particles;
    private bool activ;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0 && activ)
        {
            transform.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
            particles.GetComponent<ParticleSystem>().Play();
            activ = false;
        }
    }
    public void boom()
    {
        activ = true;
        timer = zaderjka;
    }
}

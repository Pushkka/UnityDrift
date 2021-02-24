using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_sys : MonoBehaviour
{
    private Animator Anim;
    public int health;
    public int damage_cur = 0;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(transform.position.y <= -0.15)
        {
            fallDown();
        }
    }

    public void get_damage()
    {
        damage_cur += 1;
        if (damage_cur > health)
        {
            Anim.Play("dead");
        }
        else
        {
            Anim.Play("damage_" + damage_cur);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Respawn")
        {
            fallDown();
        }
    }
    public void fallDown()
    {

        Anim.Play("fall_down");
        transform.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up * -500, transform.position + transform.forward);
        transform.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up * 500, transform.position - transform.forward);
        GetComponent<damage_sys>().enabled = false;
        GetComponent<car_controler>().stopsmoke();
        GetComponent<car_controler>().stopEmiting();
    }

}

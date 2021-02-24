using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_get : MonoBehaviour
{
    public damage_sys sys_damage;
    public float DangerPower;
    public float MaxDangerPower;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        float power = collision.relativeVelocity.magnitude;
        if (power >= DangerPower && collision.gameObject.layer != 11 && collision.gameObject.layer != 14)
        {
            if (power >= MaxDangerPower)
            {
                sys_damage.get_damage();
            }
            else
            {
                if (Mathf.Atan2(collision.contacts[0].point.y - transform.position.y, collision.contacts[0].point.x - transform.position.x) * 180 / Mathf.PI >= 75 && Mathf.Atan2(collision.contacts[0].point.y - transform.position.y, collision.contacts[0].point.x - transform.position.x) * 180 / Mathf.PI <= 145)
                {
                    sys_damage.get_damage();
                }
                else
                if (Mathf.Atan2(collision.contacts[0].point.y - transform.position.y, collision.contacts[0].point.x - transform.position.x) * 180 / Mathf.PI >= 35 && Mathf.Atan2(collision.contacts[0].point.y - transform.position.y, collision.contacts[0].point.x - transform.position.x) * 180 / Mathf.PI <= 85)
                {
                    sys_damage.get_damage();
                }
            }
            Debug.Log(power + " / " + collision.gameObject.layer + " / " + collision.gameObject.name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Cam_Height : MonoBehaviour
{
    Transform Height_Obj;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!Height_Obj)
                Height_Obj = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).transform;
            Height_Obj.localPosition = new Vector3(0,15,-7);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        Height_Obj.localPosition = new Vector3(0, 2, -3);
    }

}

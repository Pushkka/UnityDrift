using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class car_controler : MonoBehaviour
{
    [HideInInspector]
    public float cur_speed = 0;
    [HideInInspector]
    public float cur_speed2 = 0;
    [HideInInspector]
    public float cur_speed_block = 0;
    private float Max_speed_block = 0;
   [HideInInspector]
    public float cur_turn;
    [HideInInspector]
    public byte damage_power;
    public bool DriftMode_en;
    public bool DropSpeed_en;
    public GameObject Vihlop;
    public GameObject StopSignals;
    public int Gear;
    [HideInInspector]
    public bool ChangingGear;
    public float[] GearsSpeed;
    public float GearChangeRev;
    public float GearChangeSpeed;
    public float turn_speed = 10; //скорость поворота
    public float speed; //скорость
    public float Acceleration; //скорость
    public WheelCollider[] wheels;
    public TrailRenderer[] trails;
    public ParticleSystem[] smokes;
    public GameObject player_obj;
    private Vector3 curRot;


    //для отладки, удалить потом
    public player_control camera;

    private float StopSignalTime;
    private bool emiting;
    private bool onGrownd = true;
    private float emiting_rev = 0.3f;
    private float Gear_rev = 0.3f;
    private Rigidbody RB;


    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    private float driftFactor;
    void Start()
    {
        Max_speed_block = speed * 0.8f;
        //startsmoke();
        cur_turn = transform.rotation.eulerAngles.y;
        RB = GetComponent<Rigidbody>();

        if (DriftMode_en)
        {
            foreach (var item in wheels)
            {
                WheelFrictionCurve curve = new WheelFrictionCurve();

                curve.extremumSlip = item.sidewaysFriction.extremumSlip;
                curve.extremumValue = item.sidewaysFriction.extremumValue;
                curve.asymptoteSlip = item.sidewaysFriction.asymptoteSlip;
                curve.asymptoteValue = item.sidewaysFriction.asymptoteValue;
                curve.stiffness = 0;
                item.sidewaysFriction = curve;
            }
            Max_speed_block *= 0.5f;
        }
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        //checkheight();
        curRot = transform.rotation.eulerAngles;
        curRot.y = cur_turn;
        curRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curRot), turn_speed * Time.deltaTime); //поворачивает тачку на определенный угол


        //вращяет колеса
        Vector3 pos;
        Quaternion rot;
        wheels[2].GetWorldPose(out pos, out rot);
        wheels[2].transform.rotation = rot;
        wheels[3].transform.rotation = rot;
        wheels[0].GetWorldPose(out pos, out rot);
        wheels[0].transform.rotation = rot;
        wheels[1].transform.rotation = rot;


        if ((cur_speed > 0.05f || cur_speed < 0) && onGrownd)
        {
            wheels[0].transform.rotation = Quaternion.Euler(wheels[0].transform.rotation.eulerAngles.x, cur_turn, 0f);
            wheels[1].transform.rotation = Quaternion.Euler(wheels[1].transform.rotation.eulerAngles.x, cur_turn, 0f);
            //задает угол поворота, из вызова функции move
            wheels[2].motorTorque = cur_speed * 0.1f;
            wheels[3].motorTorque = cur_speed * 0.1f;

            //if (!ChangingGear)
            //{
            cur_speed_block = Mathf.Clamp(cur_speed_block, 0, cur_speed);

            if (Gear < GearsSpeed.Length)
                if (RB.velocity.magnitude > GearsSpeed[Gear])
                {
                    Gear++;
                    //Vihlop.GetComponent<DisableAfterTime>().SetTimer();
                    //Vihlop.SetActive(true);
                    GearChangeRev = Time.time + 2f;
                    ChangingGear = true;
                    Gear_rev = Time.time + 0.25f;
                    cur_speed_block += Max_speed_block * 0.75f * GearChangeSpeed;
                }
            if (Gear >= 1)
                if (RB.velocity.magnitude < GearsSpeed[Gear - 1] - 0.5f && GearChangeRev < Time.time)
                {
                    Gear--;
                    Vihlop.GetComponent<DisableAfterTime>().SetTimer();
                    Vihlop.SetActive(true);
                    ChangingGear = true;
                    Gear_rev = Time.time + 0.35f;
                    cur_speed_block += Max_speed_block * 0.5f * GearChangeSpeed;
                }
            if (cur_speed > 0)
                cur_speed -= cur_speed_block;
            RB.AddForce(transform.forward * cur_speed);
            if (Gear <= 1)
                RB.AddForce(transform.forward * speed * Acceleration);
            cur_speed = Mathf.Lerp(cur_speed, 0, Time.deltaTime * 2);


            cur_speed2 = Mathf.Lerp(cur_speed2, cur_speed - 9, Time.deltaTime * 0.5f);
            if(cur_speed2 > cur_speed && !ChangingGear)
            {
                StopSignals.SetActive(true);
                StopSignalTime = Time.time + 1;
            }
        }
        else
        {
            wheels[2].motorTorque = 0;
            wheels[3].motorTorque = 0;
        }

        WheelHit wheelHit;
        bool slipOnde = false;
        bool slipTwo = false;

        ///НАДО ДЛЯ ДРИФТА, НОРМАЛЬНОГО!!!!!!!!!!! ОДНОГО КОЛЕСА МАЛО НА СПУСКАХ!!!!!!!!
        ///
        //поиск дрифта на 2 колесе
        wheels[2].GetGroundHit(out wheelHit);
        if (DropSpeed_en)
            if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.08f)
            {
                slipOnde = true;
                emiting_rev = 1f;
                cur_speed_block = Mathf.Lerp(cur_speed_block, Max_speed_block, Time.deltaTime);
            }
        if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.15f)
        {
            emiting_rev = 0.20f;
            if (!emiting)
            {
                slipTwo = true;
                startsmoke();
                startEmiting();
            }
            cur_speed_block = Mathf.Lerp(cur_speed_block, Max_speed_block, Time.deltaTime);
        }
        wheels[3].GetGroundHit(out wheelHit);
        //поиск дрифта на 3 колесе
        if (DropSpeed_en)
            if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.08f && !slipOnde)
            {
                emiting_rev = 1f;
                cur_speed_block = Mathf.Lerp(cur_speed_block, Max_speed_block, Time.deltaTime);
            }
        if (Mathf.Abs(wheelHit.sidewaysSlip) > 0.15f && !slipTwo)
        {
            emiting_rev = 0.20f;
            if (!emiting)
            {
                startsmoke();
                startEmiting();
            }
            cur_speed_block = Mathf.Lerp(cur_speed_block, Max_speed_block, Time.deltaTime);
        }



        if(Time.time > StopSignalTime)
            StopSignals.SetActive(false);

        if (Time.time > Gear_rev)
                ChangingGear = false;

        emiting_rev -= Time.deltaTime;
        if (emiting_rev < 0 || RB.velocity.magnitude < 6)
        {
            cur_speed_block = Mathf.Lerp(cur_speed_block, 0, Time.deltaTime * 2);
            if (emiting)
            {
                stopsmoke();
                stopEmiting();
                Vihlop.GetComponent<DisableAfterTime>().SetTimer();
                Vihlop.SetActive(true);
            }
        }
    }
    void checkheight()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position+ transform.up * 0.025f, transform.up*-0.8f, out hit, 2f))
        {
            stopsmoke();
            stopEmiting();
            onGrownd = false;
            RB.drag = 0.25f;
            RB.angularDrag = 0.05f;
        }
        else
        {
            onGrownd = true;
            //RB.drag = 1;
            //RB.angularDrag = 100;
        }
    }
    public void startsmoke()
    {
        foreach (ParticleSystem t in smokes)
        {
            t.Play();
        }
    }
    public void stopsmoke()
    {
        foreach (ParticleSystem t in smokes)
        {
            t.Stop();
        }
    }
    public void startEmiting()
    {
        emiting = true;
        foreach (TrailRenderer t in trails)
        {
            t.emitting = true;
        }
    }
    public void stopEmiting()
    {
        emiting = false;
        foreach (TrailRenderer t in trails)
        {
            t.emitting = false;
        }
    }
    public void Move(float rot, float angle, float speed)
    {
        cur_speed = speed;
        cur_turn = rot;
        //RB.drag = 1;
        wheels[0].brakeTorque = 0;
        wheels[1].brakeTorque = 0;
        wheels[2].brakeTorque = 0;
        wheels[3].brakeTorque = 0;
    }
    public void Stop()
    {
        RB.drag = 2;
        cur_speed = 0;
        wheels[0].brakeTorque = speed * 4f;
        wheels[1].brakeTorque = speed * 4f;
        wheels[2].brakeTorque = speed * 2f;
        wheels[3].brakeTorque = speed * 2f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_control : MonoBehaviour
{
    public GameObject cam;
    public Text indikator;
    public RectTransform centr;
    public Image touch;
    private Vector2 touch_kord;
    private Vector2 center_screen;
    public float angle;
    public float m_Angle;
    public float cam_angle;
    public float cam_turn_speed;
    public float Distance;
    public float Res_Modif;
    public GameObject car;
    private car_controler car_contr;
    public List<GameObject> cop_points;
    GameObject[] all_points;

    void Start()
    {
        car_contr = car.GetComponent<car_controler>();
        resetCenter();
         cam_angle = transform.rotation.eulerAngles.y;
        all_points = GameObject.FindGameObjectsWithTag("trafic_point");

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 3000;
        Res_Modif = 1080 / (float)Screen.width;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(0, cam_angle, 0), cam_turn_speed * Time.deltaTime);
    }
    int everyFiveTeenframes = 0;
    int everyThreeTeenframes = 0;
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            touch_kord = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Distance = Vector2.Distance(touch_kord, center_screen) * Res_Modif;
            Distance = Mathf.Clamp(Distance,250,450);
            Distance /= 450;
            indikator.text = "x = " + touch_kord.x + " y = " + touch_kord.y;

            angle = Mathf.Atan2(touch_kord.y - center_screen.y, touch_kord.x - center_screen.x) * 180 / Mathf.PI;
            if (angle < 0)
                angle += 360;

            Vector3 curRot = transform.rotation.eulerAngles;
            m_Angle = curRot.y - angle - 90;

            centr.rotation = Quaternion.Euler(0, 0, angle + 90f);
            cam_angle = m_Angle;

            car_contr.Move(m_Angle, angle, car_contr.speed * Distance);
        }


        everyThreeTeenframes += 1;
        if (everyThreeTeenframes >= 30)
        {
            //scan_spawn_trafic();
            //delete_trafic();
            everyFiveTeenframes = 0;
        }
    }
    public void resetCenter()
    {
        center_screen = new Vector2(Screen.width / 2, Screen.height / 3);
        centr.position = new Vector3(Screen.width / 2, Screen.height / 3);
        touch_kord = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 25f);
    }
}


using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float Delay = 0;
    public float Timer = 0;
    void Awake()
    {
        SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > Timer)
            gameObject.SetActive(false);
    }
    public void SetTimer()
    {
        Timer = Time.time + Delay;
    }
}

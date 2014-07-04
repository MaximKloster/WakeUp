using UnityEngine;
using System.Collections;

public class NeonlightController : MonoBehaviour
{
    [SerializeField]
    bool on = true;
    [SerializeField]
    bool flicker = false;
    [Range(0, 1)]
    [SerializeField]
    float flickerChance = 0.1f;
    [SerializeField]
    float flickerTime = -1;

    public bool On { get { return on; } set { on = value; } }
    public bool Flicker
    {
        get { return flicker; }
        set
        {
            flicker = value;
            flickerStartTime = Time.time;
        }
    }

    GameObject light;
    float flickerStartTime, disableStartTime;
    bool disableSeries;
    int seriesLightCount;

    // Use this for initialization
    void Start()
    {
        if (transform.FindChild("Light") != null)
            light = transform.FindChild("Light").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (light != null)
        {
            light.SetActive(on);

            if (flicker && Random.value > 1 - flickerChance
                && (flickerStartTime + flickerTime > Time.time || flickerTime < 0))
                light.SetActive(!light.activeSelf);
        }

        if (disableSeries && seriesLightCount > 1)
            foreach (Transform lightChild in GetComponentInChildren<Transform>())
                if (lightChild.gameObject.transform.FindChild("Light") != null
                    && lightChild.gameObject.transform.FindChild("Light").gameObject.activeSelf)
                {
                    float toFloat;
                    if (!float.TryParse(lightChild.gameObject.name, out toFloat)) return;

                    if (disableStartTime + toFloat < Time.time)
                    {
                        lightChild.gameObject.transform.FindChild("Light").gameObject.SetActive(false);
                        seriesLightCount--;
                    }
                }
    }

    public void DisableSeries()
    {
        disableStartTime = Time.time;
        seriesLightCount = transform.childCount;
        disableSeries = true;
    }
}
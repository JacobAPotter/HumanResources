using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material yellowScribble { get; private set; }
    public Material blueScribble { get; private set; }

    float scribbleTimeStamp;
    const float scribbleInterval = 0.133f;

    void Start()
    {
        Transform scrib = transform.Find("YellowScribble");
        yellowScribble = scrib.GetComponent<Renderer>().sharedMaterial;
        yellowScribble.SetTextureScale("_MainTex", new Vector2(0.2f, 1.0f));
        scrib.gameObject.SetActive(false);

        scrib = transform.Find("BlueScribble");
        blueScribble = scrib.GetComponent<Renderer>().sharedMaterial;
        blueScribble.SetTextureScale("_MainTex", new Vector2(0.2f, 1.0f));

        scrib.gameObject.SetActive(false);

    }

    void Update()
    {

        if (Time.timeSinceLevelLoad - scribbleTimeStamp > scribbleInterval)
        {
            scribbleTimeStamp = Time.timeSinceLevelLoad;
            yellowScribble.mainTextureOffset = Vector2.right * Random.value;
            blueScribble.mainTextureOffset = Vector2.right * Random.value;
        }

    }
}

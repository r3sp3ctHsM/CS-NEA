using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public double beatTempo;

    public bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            /*if (Input.anyKeyDown)
            {
                hasStarted = true;
            } */
        }
        else
        {
            transform.position -= new Vector3(0f, (Convert.ToSingle(beatTempo) / 60f) * Time.deltaTime, 0f);
        }
    }
}

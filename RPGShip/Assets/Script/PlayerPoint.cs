﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        this.transform.position = new Vector3( //
            ContentManager.instance.nowMapPoint.transform.position.x, //
            ContentManager.instance.nowMapPoint.transform.position.y, //
            this.transform.position.z
            );
    }

    // Update is called once per frame
    void Update()
    {

    }
}

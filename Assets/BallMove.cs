using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform mything = GetComponent<Transform>();

        //var pos = mything.position;
        //pos.x = 5;
        transform.Translate(Vector3.back * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeMove : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> cubes;

    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);
        var cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.transform.position = new Vector3(0, 0.5f, 0);
        var cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(0, 0.5f, 0);
        cubes = new List<GameObject>();
        cubes.Add(cube);
        cubes.Add(cube1);
        cubes.Add(cube2);
    }

    // Update is called once per frame
    void Update()
    {

        cube.transform.Translate(Vector3.back * Time.deltaTime);

        var pos = cube.transform.position;
        if (pos.z < -7)
        {
            cubes.Remove(cube);
            Destroy(cube);
            cube = cubes[cubes.Count - 1];
        }

    }
}

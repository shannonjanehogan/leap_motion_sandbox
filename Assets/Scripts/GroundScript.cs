using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundScript : MonoBehaviour
{
    //public GameObject cube;
    public List<GameObject> shapeList;
    public GameObject verticalShapePrefab;
    public GameObject verticalShape;

    // Start is called before the first frame update
    void Start()
    {
        verticalShape = Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        //shape1.transform.position = new Vector3(0, 0.5f, 0);
        shapeList = new List<GameObject>();
        shapeList.Add(verticalShape);

        Debug.Log(verticalShape.transform);
    }

    // Update is called once per frame
    void Update()
    {
        verticalShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);

        var pos = verticalShape.transform.position;

        if (pos.z < -11)
        {
            shapeList.Remove(verticalShape);
            Destroy(verticalShape);
            verticalShape = shapeList[shapeList.Count - 1];
        }

    }
}

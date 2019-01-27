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
        shapeList = new List<GameObject>();
        shapeList.Add(verticalShape);

        Debug.Log(verticalShape.transform);
    }

    // Update is called once per frame
    void Update()
    {
        verticalShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);
        verticalShape.transform.localScale -= ScaleVector(verticalShape.transform.localScale, 0.003f);

        var pos = verticalShape.transform.position;
        verticalShape.transform.position = new Vector3(pos.x, 0, pos.z);

        if (pos.z < -11)
        {
            shapeList.Remove(verticalShape);
            Destroy(verticalShape);
            verticalShape = shapeList[shapeList.Count - 1];
        }

    }

    Vector3 ScaleVector(Vector3 localScale, float factor)
    {
        float xScale = localScale.x * factor;
        float yScale = localScale.y * factor;
        float zScale = localScale.z * factor;

        return new Vector3(xScale, yScale, zScale);
    }
}

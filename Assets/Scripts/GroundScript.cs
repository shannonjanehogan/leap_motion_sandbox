using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundScript : MonoBehaviour
{
    public List<GameObject> shapeList;
    public GameObject verticalShapePrefab;
    public GameObject verticalShape;
    public GameObject yShapePrefab;
    public GameObject yShape;
    public GameObject lShapePrefab;
    public GameObject lShape;
    public GameObject currShape;
    public GameObject nextShape;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all shapes
        verticalShape = Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        yShape = Instantiate(yShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        lShape = Instantiate(lShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        // Add shapes to list of shapes
        shapeList = new List<GameObject>();
        shapeList.Add(verticalShape);
        shapeList.Add(yShape);
        shapeList.Add(lShape);

        // Set the current and next Shapes
        currShape = verticalShape;
        nextShape = lShape;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the current shape forwards
        currShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);
        currShape.transform.localScale -= ScaleVector(currShape.transform.localScale, 0.003f);
        var pos = currShape.transform.position;
        currShape.transform.position = new Vector3(pos.x, 0, pos.z);

        if (shapeList.Count > 1)
        {
            nextShape.transform.Translate(Vector3.back * (Time.deltaTime * 15), Space.World);
        }

        // Once the current shape passes the camera, destroy it, then update the current shape
        if (pos.z < -10)
        {
            shapeList.Remove(currShape);
            Destroy(currShape);
            currShape = shapeList[shapeList.Count - 1];
            if (shapeList.Count > 1)
            {
                nextShape = shapeList[shapeList.Count - 2];
            }
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

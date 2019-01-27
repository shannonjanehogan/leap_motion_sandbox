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
    public GameObject currShape;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all shapes
        verticalShape = Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        yShape = Instantiate(yShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        // Add shapes to list of shapes
        shapeList = new List<GameObject>();
        shapeList.Add(verticalShape);
        shapeList.Add(yShape);

        // Set the current Shape
        currShape = verticalShape;
    }

    // Update is called once per frame
    void Update()
    {
        currShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);
        var pos = currShape.transform.position;

        if (pos.z < -11)
        {
            shapeList.Remove(currShape);
            Destroy(currShape);
            currShape = shapeList[shapeList.Count - 1];
        }

    }
}

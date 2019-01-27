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
    public GameObject extendedShape;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all shapes
        verticalShape = Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        yShape = Instantiate(yShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        lShape = Instantiate(lShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));

        // Add shapes to list of shapes
        shapeList = new List<GameObject> { verticalShape, lShape, yShape };

        SetInitialShapeColor();

        // Set the current and next Shapes
        currShape = verticalShape;
        MakeExtendedShape(currShape);
        nextShape = lShape;
    }

    void HandleCollisionStarted()
    {
        ChangeShapeColor(currShape, Color.red);
    }

    void HandleCollisionEnded()
    {
        ChangeShapeColor(currShape, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if (currShape)
        {
            // Move the current shape forwards
            currShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);
            ScaleShape(currShape, 0.003f, true);
            ScaleShape(extendedShape, 0.003f, true);
            var pos = currShape.transform.position;
            currShape.transform.position = new Vector3(pos.x, 0.5f, pos.z);

            if (shapeList.Count > 1)
            {
                nextShape.transform.Translate(Vector3.back * (Time.deltaTime * 15), Space.World);
                ScaleShape(nextShape, 0.003f, false);
            }

            // Once the current shape passes the camera, destroy it, then update the current shape
            if (pos.z < -10)
            {
                shapeList.Remove(currShape);
                Destroy(currShape);
                if (shapeList.Count > 0)
                {
                    currShape = shapeList[0];
                    MakeExtendedShape(currShape);
                }
                if (shapeList.Count > 1)
                {
                    nextShape = shapeList[1];
                }
            }
        }
    }

    void MakeExtendedShape(GameObject original)
    {
        if (extendedShape)
        {
            Destroy(extendedShape);
        }
        extendedShape = Instantiate(original) as GameObject;

        //Set alpha
        var material = new Material(Shader.Find("Standard"));

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Color color = Color.gray;
        color.a = 0.3f;
        material.color = color;
        extendedShape.GetComponent<MeshRenderer>().material = material;

        //Scale it
        Vector3 scale = extendedShape.transform.localScale;
        scale.y = 2000;
        extendedShape.transform.localScale = scale;
    }
   
    void SetInitialShapeColor()
    {
        List<Color> colors = new List<Color> { Color.black, Color.black, Color.black };

        for(int i = 0; i < shapeList.Count; i++)
        {
            ChangeShapeColor(shapeList[i], colors[i]);
        }
    }

    void ChangeShapeColor(GameObject gameObject, Color color)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", color);
        //Find the Specular shader and change its Color to red
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.grey);
    }

    void ScaleShape(GameObject gameObject, float factor, bool extended)
    {
        var curScale = gameObject.transform.localScale;
        float xScale = curScale.x * factor;
        float yScale = extended ? 0 : curScale.y * factor;
        float zScale = curScale.z * factor;

        gameObject.transform.localScale -= new Vector3(xScale, yScale, zScale);
    }
}

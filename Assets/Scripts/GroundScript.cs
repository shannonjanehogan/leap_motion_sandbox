using Leap.Unity.Interaction;
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
    public GameObject cShapePrefab;
    public GameObject cShape;
    public GameObject dShapePrefab;
    public GameObject dShape;
    public GameObject currShape;
    public GameObject nextShape;
    public GameObject extendedShape;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all shapes
        verticalShape = Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        yShape = Instantiate(yShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        lShape = Instantiate(lShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        cShape = Instantiate(cShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        dShape = Instantiate(dShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));

        var thingy = lShape.GetComponent<InteractionBehaviour>();

        // Add shapes to list of shapes
        shapeList = new List<GameObject> { verticalShape, lShape, yShape, cShape, dShape };

        SetInitialShapeColor();

        // Set the current and next Shapes
        currShape = verticalShape;
        MakeExtendedShape(currShape);
        nextShape = lShape;
        SetScoreText("Score: 0");
    }

    public void HandleCollisionStarted()
    {
        ChangeShapeColor(currShape, Color.red);
    }

    public void HandleCollisionEnded()
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
            ScaleShape(currShape, 0.002f, true);
            ScaleShape(extendedShape, 0.002f, true);
            var pos = currShape.transform.position;
            foreach (GameObject shape in shapeList)
            {
                AdjustVerticalPosition(shape, 0);
            }
            AdjustVerticalPosition(extendedShape, 0);

            if (shapeList.Count > 1)
            {
                nextShape.transform.Translate(Vector3.back * (Time.deltaTime * 15), Space.World);
                ScaleShape(nextShape, 0.003f, false);
            }

            // Once shape is past hands, fade it out
            if (pos.z < -9)
            {
                FadeOutObject(currShape);
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
        var material = TransparentMaterial();

        Color color = Color.gray;
        color.a = 0.3f;
        material.color = color;
        extendedShape.GetComponent<MeshRenderer>().material = material;

        //Scale it
        Vector3 scale = extendedShape.transform.localScale;
        scale.y = 2000;
        extendedShape.transform.localScale = scale;
    }

    void FadeOutObject(GameObject shape)
    {
        var curColor = shape.GetComponent<MeshRenderer>().material.color;
        var material = TransparentMaterial();
        curColor.a *= 0.95f;
        material.color = curColor;
        shape.GetComponent<MeshRenderer>().material = material;
    }

    Material TransparentMaterial()
    {
        var material = new Material(Shader.Find("Standard"));

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        return material;
    }
   
    // Set color of all shapes to black
    void SetInitialShapeColor()
    {
        foreach (GameObject shape in shapeList)
        {
            var material = TransparentMaterial();
            ChangeShapeColor(shape, Color.black);
        }
    }

    public void SetNewScore(int newScore)
    {
        score += newScore;
        SetScoreText(score.ToString());
    }

    // Changes the text of the Score Text UI component to the given string
    void SetScoreText(string newText)
    {
        TextMesh textMesh = (TextMesh) GameObject.Find("Score Text").GetComponent<TextMesh>();
        textMesh.text = newText;
    }

    // Given a Game Object and a Color, changes the color of the Game Object to match the given color
    void ChangeShapeColor(GameObject gameObject, Color color)
    {
        if (gameObject == null)
        {
            return;
        }
        var material = gameObject.GetComponent<MeshRenderer>().material;
        material.color = color;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    void ScaleShape(GameObject gameObject, float factor, bool extended)
    {
        var curScale = gameObject.transform.localScale;
        float xScale = curScale.x * factor;
        float yScale = extended ? 0 : curScale.y * factor;
        float zScale = curScale.z * factor;

        gameObject.transform.localScale -= new Vector3(xScale, yScale, zScale);
    }

    void AdjustVerticalPosition(GameObject gameObject, float desiredY)
    {
        var pos = gameObject.transform.position;
        var boundingBoxYSize = gameObject.GetComponent<MeshRenderer>().bounds.size.y;
        var adjustedY = desiredY + (boundingBoxYSize / 2);

        gameObject.transform.position = new Vector3(pos.x, adjustedY, pos.z);
    }
}

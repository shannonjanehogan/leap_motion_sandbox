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
    public System.Random random;
    public int score = 0;
    public int curIdx;
    public int nextIdx;
    public List<GameObject> prefabs;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random(587);
        // Instantiate all shapes

        prefabs = new List<GameObject>
        {
            verticalShapePrefab,
            yShapePrefab,
            lShapePrefab,
            cShapePrefab,
            dShapePrefab
        };
        
        shapeList = new List<GameObject>
        {
            Instantiate(verticalShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right)),
            Instantiate(yShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right)),
            Instantiate(lShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right)),
            Instantiate(cShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right)),
            Instantiate(dShapePrefab, new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right))
        };

        foreach (GameObject shape in shapeList)
        {
            SetInitialShapeColor(shape);
        }

        // Set the current and next Shapes
        curIdx = 0;
        nextIdx = random.Next(1, shapeList.Count-1);
        currShape = shapeList[curIdx];
        MakeExtendedShape(currShape);

        nextShape = shapeList[nextIdx];
        SetScoreText("Score: 0");
    }

    void GetNewShapes()
    {
        curIdx = nextIdx;
        currShape = nextShape;
        var rand = curIdx;
       while (rand == curIdx || rand == nextIdx)
        {
            rand = random.Next(shapeList.Count);
        }
        nextIdx = rand;
        var newShape = Instantiate(prefabs[nextIdx], new Vector3(0, 0.5f, 0), Quaternion.AngleAxis(90, Vector3.right));
        SetInitialShapeColor(newShape);
        nextShape = newShape;
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
            ScaleShape(currShape, 0.0038f, true);
            ScaleShape(extendedShape, 0.0038f, true);
            var pos = currShape.transform.position;
            foreach (GameObject shape in shapeList)
            {
                AdjustVerticalPosition(shape, 0);
            }
            AdjustVerticalPosition(extendedShape, 0);

            if (nextShape && pos.z < -4.5)
            {
                nextShape.transform.Translate(Vector3.back * (Time.deltaTime * 20), Space.World);
                ScaleShape(nextShape, 0.0038f, false);
            }

            // Once shape is past hands, fade it out
            if (pos.z < -9)
            {
                FadeOutObject(currShape);
                FadeOutObject(extendedShape);
            }

            // Once the current shape passes the camera, destroy it, then update the current shape
            if (pos.z < -10)
            {
                //shapeList.Remove(currShape);
                //Destroy(currShape);
                GetNewShapes();
                MakeExtendedShape(currShape);
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
    void SetInitialShapeColor(GameObject shape)
    {
        var material = TransparentMaterial();
        ChangeShapeColor(shape, Color.black);
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

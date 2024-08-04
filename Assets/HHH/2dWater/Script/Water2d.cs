using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Water2d : MonoBehaviour
{

    //public GameObject splash;
    public Material mat;

    public GameObject waterSprite;

    public float waterWidth;
    public float waterHeight;
    //
    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;
    LineRenderer Body;
    //
   
    GameObject[] spriteShapeObjects;
    SpriteShapeController[] spriteShapeControllers;
    //
    GameObject[] colliders;
    //
    const float springconstant = 0.02f;
    const float damping = 0.25f;
    const float spread = 0.05f;
    const float z = 0f;
    //
    float baseheight;
    float left;
    float bottom;
    //

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //float halfHeight = waterHeight / 2f;
        float halfWidth = waterWidth / 2f;

        Vector3 topLeft = transform.position + new Vector3(-halfWidth, 0f, 0f);
        Vector3 topRight = transform.position + new Vector3(halfWidth, 0f, 0f);
        Vector3 downLeft = transform.position + new Vector3(-halfWidth, -waterHeight, 0f);
        Vector3 downRight = transform.position + new Vector3(halfWidth, -waterHeight, 0f);


        Gizmos.DrawLine(topLeft,topRight);
        Gizmos.DrawLine(topLeft,downLeft);
        Gizmos.DrawLine(downRight, topRight);
        Gizmos.DrawLine(downRight, downLeft);

    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnWater();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < xpositions.Length; i++)
        {
            float force = springconstant * (ypositions[i] - baseheight) + velocities[i] * damping;
            accelerations[i] = -force;
            ypositions[i] += velocities[i];
            velocities[i] += accelerations[i];
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
        }
       
        //
        float[] leftDeltas = new float[xpositions.Length];
        float[] rightDeltas = new float[xpositions.Length];
        //
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = spread * (ypositions[i] - ypositions[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xpositions.Length - 1)
                {
                    rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }
        }

        for (int i = 0; i < xpositions.Length; i++)
        {
            if (i > 0)
            {
                ypositions[i - 1] += leftDeltas[i];
            }
            if (i < xpositions.Length - 1)
            {
                ypositions[i + 1] += rightDeltas[i];
            }
        }

        UpdateMeshes();
    }
    public void SpawnWater()
    {
        float Left = -waterWidth/2f;
        float Width = waterWidth;
        float Top = 0f;
        float Bottom = -waterHeight;
        //
        int edgecount = Mathf.RoundToInt(Width) * 5;
        int nodecount = edgecount + 1;

        Body = gameObject.AddComponent<LineRenderer>();

        Body.useWorldSpace = false;
        Body.material.renderQueue = 1000;
        Body.positionCount = nodecount;
        Body.startWidth = Body.endWidth = 0.1f;
        Body.sortingLayerName = "Water";
        Body.material = mat;


        xpositions = new float[nodecount];
        ypositions = new float[nodecount];
        velocities = new float[nodecount];
        accelerations = new float[nodecount];

        
        spriteShapeObjects = new GameObject[edgecount];
        spriteShapeControllers = new SpriteShapeController[edgecount];
        colliders = new GameObject[edgecount];

        baseheight = Top;
        bottom = Bottom;
        left = Left;

        for (int i = 0; i < nodecount; i++)
        {
            ypositions[i] = Top;
            xpositions[i] = Left + Width * i / edgecount;
            accelerations[i] = 0;
            velocities[i] = 0;
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));

        }


        for (int i = 0; i < edgecount; i++)
        {
           

            Vector3[] Vertices = new Vector3[4];
            Vertices[1] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[0] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], bottom, z);
            Vertices[3] = new Vector3(xpositions[i + 1], bottom, z);
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            spriteShapeObjects[i] = Instantiate(waterSprite, Vector3.zero + transform.position, Quaternion.identity);
            SpriteShapeController controller = spriteShapeObjects[i].GetComponent<SpriteShapeController>();
            spriteShapeControllers[i] = controller;
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    controller.spline.SetPosition(j, Vertices[j]);
                }
                catch
                {
                }
            }
           
            spriteShapeObjects[i].transform.parent = transform;

            //

            colliders[i] = new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            colliders[i].transform.localPosition = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.1f, 0);
            colliders[i].transform.localScale = new Vector3(Width / edgecount, 0.2f, 1);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<Water2dDetect>();


        }
    }


    void UpdateMeshes()
    {
        for (int i = 0; i < spriteShapeControllers.Length; i++)
        {

            Vector3[] Vertices = new Vector3[4];
            Vertices[1] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[0] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], bottom, z);
            Vertices[3] = new Vector3(xpositions[i + 1], bottom, z);

            SpriteShapeController controller = spriteShapeControllers[i];
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    controller.spline.SetPosition(j, Vertices[j]);
                }
                catch
                {
                    
                }
            }
        }
    }

    public void Splash(float xpos, float velocity)
    {
        if (xpos >= xpositions[0]+transform.position.x && xpos <= xpositions[xpositions.Length - 1]+transform.position.x)
        {

            xpos -= xpositions[0] + transform.position.x;

            int index = Mathf.RoundToInt((xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));
            velocities[index] = velocity;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SimpleWater : MonoBehaviour
{

    private GameObject PopParticle;
    private float PopGenerateTimer;
    public GameObject waterSprite;
    private List<GameObject> flowWaterList = new();

    private List<GameObject> viewSprites = new();


    public float waterWidth;
    public float waterHeight;
    public bool InitView = true;
    BoxCollider2D waterCollider;
    public bool IsHoldNeeded = true;
    private bool isActive;
    private float ActiveTimer;
    private PlayerMgr templeMgr;
    private const float BuoyancyForce = 50;
    private const float WaterResistance = 2f;
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

        Color color = new Color(0, 0.3f, 1f, 1f);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topLeft, downLeft, color);
        Debug.DrawLine(downRight, topRight, color);
        Debug.DrawLine(downRight, downLeft, color);

        //Vector3 pos = transform.position;
        //pos = new Vector3(Mathf.FloorToInt(pos.x)+0.5f, Mathf.FloorToInt(pos.y) - 0.5f, 0);
        //GetWaterOffSetAndSize(pos, out float offset, out Vector2 size);
    }
    // Start is called before the first frame update

    void Start()
    {
        PopParticle = Resources.Load<GameObject>("Pop");
        waterCollider = GetComponent<BoxCollider2D>();
        waterCollider.size = new Vector2(waterWidth, waterHeight);
        waterCollider.offset = new Vector2(0, -waterHeight * 0.5f);

        float halfWidth = waterWidth / 2f;
        Vector3 topLeft = transform.position + new Vector3(-halfWidth, 0f, 0f);
        Vector3 topRight = transform.position + new Vector3(halfWidth, 0f, 0f);
        if(InitView)
        for (int i = 0; i < waterWidth; i++)
        {
            for (int j = 0; j < waterHeight; j++)
            {
                GameObject go = Instantiate(waterSprite, transform);
                go.transform.position = topLeft + new Vector3(0.5f, -0.5f) + Vector3.right * i + Vector3.down * j;
                viewSprites.Add(go);
            }

        }

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.TryGetComponent(out FlowWater flowWater))
            {
                flowWaterList.Add(child);
            }
        }
        PopGenerateTimer = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) PlayerExit();


        if (ActiveTimer + 0.1f < Time.time)
        {
            Disappear();
        }
        else
        {
            Appear();
        }
    }
    public void Disappear()
    {
        if (IsHoldNeeded == false) return;

        if (!isActive) return;
        isActive = false;
        foreach (var item in viewSprites)
        {
            item.SetActive(false);
        }
        foreach (var item in flowWaterList)
        {
            item.SetActive(false);
        }
    }
    public void Appear()
    {
        if (isActive) return;
        isActive = true;
        foreach (var item in viewSprites)
        {
            item.SetActive(true);
        }
        foreach (var item in flowWaterList)
        {
            item.SetActive(true);
        }
    }
    public void Hold()
    {
        ActiveTimer = Time.time;
    }

    private void FixedUpdate()
    {
        if (templeMgr != null)
        {
            

            float distance = transform.position.y - templeMgr.transform.position.y + 1;
            distance = Mathf.Clamp(distance, 0, 1);
            Rigidbody2D rb = templeMgr.moveController.GetRigidBody();
            if (templeMgr.battery == null)
                rb.AddForce(new Vector2(0, BuoyancyForce * distance));
            rb.AddForce(-WaterResistance * rb.velocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.CompareTag("Player"))
        {
            PlayerMgr mgr = collision.GetComponent<PlayerMgr>();
            mgr.moveController.WaterTouchedCount += 1;
            GeneratePop(mgr.transform.position + Vector3.down);
            templeMgr = mgr;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerExit();
        }
    }
    public void PlayerExit()
    {
        if (templeMgr != null)
        {

            templeMgr.moveController.WaterTouchedCount -= 1;
            GeneratePop(templeMgr.transform.position + Vector3.down);
            templeMgr = null;
        }
    }
    public void GeneratePop(Vector3 pos)
    {
        if (PopGenerateTimer + 0.1f > Time.time) return;
        PopGenerateTimer = Time.time;

        GameObject popPtc = ObjectPool.Instance.GetObject(PopParticle);
        popPtc.transform.position = pos;
    }
    [ContextMenu("Resize")]
    public void Resize()
    {

        Vector3 pos = transform.position;
        pos = new Vector3(Mathf.FloorToInt(pos.x) + 0.5f, Mathf.FloorToInt(pos.y) - 0.5f, 0);
        GetWaterOffSetAndSize(pos, out float offset, out Vector2 size);
        waterWidth = size.x;

        transform.position = pos + Vector3.up * 0.5f + offset * Vector3.right;

    }
    [ContextMenu("DeleteFlowWater")]
    public void DeleteFlowWater()
    {
        //delete
        List<GameObject> obj = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.TryGetComponent(out FlowWater flowWater))
            {
                obj.Add(child);
            }
        }
        for (int i = 0; i < obj.Count; i++)
        {
            DestroyImmediate(obj[i]);
        }
    }

    [ContextMenu("ReSetFlowWater")]
    public void ResetWater()
    {
        DeleteFlowWater();
        //load Pos
        Vector3 pos = transform.position;
        pos = new Vector3(Mathf.FloorToInt(pos.x) + 0.5f, Mathf.FloorToInt(pos.y) - 0.5f, 0);
        GetWaterOffSetAndSize(pos, out float leftX, out float rightX);

        //waterPos

        //left
        GameObject flowWaterPrefab = Resources.Load<GameObject>("FlowWater");
        GameObject leftWater = Instantiate(flowWaterPrefab, transform);
        GameObject rightWater = Instantiate(flowWaterPrefab, transform);
        leftWater.transform.position = new Vector3(leftX - 0.5f, pos.y - 1, 0);
        rightWater.transform.position = new Vector3(rightX + 0.5f, pos.y - 1, 0);

    }
    private void GetWaterOffSetAndSize(Vector2 center, out float left, out float right)
    {
        GetWaterOffSetAndSize(center, out float offset, out Vector2 size);
        left = center.x - size.x / 2f + offset;
        right = center.x + size.x / 2f + offset;

    }
    private void GetWaterOffSetAndSize(Vector2 center, out float offset, out Vector2 size)
    {
        float right = center.x + 0.5f;
        float left = center.x - 0.5f;
        const int MaxCheckWidth = 50;

        int leftMaxCheckRange = MaxCheckWidth;
        int rightMaxCheckRange = MaxCheckWidth;
        RaycastHit2D leftHit;
        RaycastHit2D rightHit;

        if (rightHit = Physics2D.Raycast(center, Vector2.right, MaxCheckWidth, 1 << 7))
        {
            right = rightHit.point.x;
            rightMaxCheckRange = (int)(rightHit.point.x - center.x);

        }
        if (leftHit = Physics2D.Raycast(center, Vector2.left, MaxCheckWidth, 1 << 7))
        {
            left = leftHit.point.x;
            leftMaxCheckRange = (int)(center.x - leftHit.point.x);

        }

        for (int i = 1; i <= rightMaxCheckRange; i++)
        {
            Vector2 origin = center + Vector2.right * i;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1, 1 << 7);

            if (!hit)
            {
                right = center.x + i - 0.5f;

                break;
            }

        }
        for (int i = 1; i <= leftMaxCheckRange; i++)
        {
            Vector2 origin = center + Vector2.left * i;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1, 1 << 7);

            if (!hit)
            {
                left = center.x - i + 0.5f;

                break;
            }

        }
        Debug.DrawLine(new Vector3(left, center.y, 0), new Vector3(right, center.y, 0));

        size = new Vector2(right - left, 1);
        offset = (right + left) / 2f - center.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class FlowWater : MonoBehaviour
{

    private BoxCollider2D _collider;
    public int waterHeight;

    public GameObject SpriteObject;
    public GameObject waterEndParticle;
    private GameObject waterEndParticleTemp;
    private PlayerMgr player;
    private float force = 7;
    private bool isTouchTheGround;
    private const int MaxCheckHeight = 100;
    private const int MaxCheckWidth = 50;
    private List<GameObject> spriteList = new List<GameObject>();
    int layerMask = (1 << 7);
    public Vector3 Center { get { return transform.position + new Vector3(0.5f, 0.5f, 0f); } }
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }
    private void Start()
    {
        NormalizeThePosition();
        InvokeRepeating(nameof(OnUpdatePerSecond), 0, 1);
    }
    private void Update()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Center, Vector2.down, MaxCheckHeight, layerMask);
        if (hit)
        {
            RaycastHit2D hitWater = Physics2D.Raycast(Center, Vector2.down, (Center.y - hit.point.y), 1 << 4);
            if (hitWater)
                if (hitWater.collider.TryGetComponent(out SimpleWater water))
                {
                    water.Hold();
                }
            int newHeight = (int)(transform.position.y + 0.4f - hit.point.y);
            if(waterHeight != newHeight) Invoke(nameof(UpdateView),0.01f);
            waterHeight = newHeight;
            GenerateParticleAtEnd();

           
        }
        else
        {
            waterHeight = 1;
            if (waterEndParticleTemp != null)
                waterEndParticleTemp.SetActive(false);
        }





        _collider.size = new Vector2(1, waterHeight);
        _collider.offset = new Vector2(0.5f, -(float)(waterHeight - 1) / 2 + 0.5f);
    }
    public void OnUpdatePerSecond()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            Destroy(spriteList[i]);
        }
        spriteList.Clear();
        for (int i = 0; i <= waterHeight; i++)
        {
            GameObject sprite = Instantiate(SpriteObject);
            sprite.transform.SetParent(transform);
            sprite.transform.position = Center + Vector3.down * i;
            spriteList.Add(sprite);
        }


    }
    public void NormalizeThePosition()
    {

        Vector3 newPos = new Vector3(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        transform.position = newPos;

    }
    public void GenerateParticleAtEnd()
    {
        if (waterEndParticleTemp == null)
        {
            GameObject go = ObjectPool.Instance.GetObject(waterEndParticle);
            go.transform.SetParent(this.transform);
            waterEndParticleTemp = go;
        }
        waterEndParticleTemp.SetActive(true);
        waterEndParticleTemp.transform.position = Center - Vector3.up * waterHeight;
    }





    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }
        Rigidbody2D rb = player.moveController.GetRigidBody();
        rb.AddForce(new Vector2(0, -force));
        rb.AddForce(1f * -rb.velocity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerMgr>();
            player.moveController.WaterTouchedCount += 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.moveController.WaterTouchedCount -= 1;
            player = null;
        }
    }
}

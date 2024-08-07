using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class FlowWater : MonoBehaviour
{
    private BoxCollider2D _collider;
    int waterHeight;
    public GameObject SpriteObject;
    public GameObject waterEndParticle;
    private PlayerMgr player;
    private float force = 7;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        RaycastHit2D hit;
        float distance = 0;
        if (hit = Physics2D.Raycast(transform.position, Vector2.down, 100, 1 << 7))
        {
            distance = transform.position.y - hit.point.y;

        }
        waterHeight = (int)distance;
        _collider.size = new Vector2(1, waterHeight + 1);
        _collider.offset = new Vector2(0, -(float)waterHeight / 2);
        InitView();
    }   
    public void InitView()
    {
        for (int i = 0; i < waterHeight + 1; i++)
        {
            GameObject go = Instantiate(SpriteObject, transform);
            go.transform.position = transform.position + new Vector3(0, -i, 0);
        }
        GameObject particle = Instantiate(waterEndParticle, transform);
        particle.transform.position = transform.position + new Vector3(0, -waterHeight, 0);
    }
    

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        RaycastHit2D hit;
        float distance = 0;
#if UNITY_EDITOR
        if (hit = Physics2D.Raycast(transform.position, Vector2.down,100,(1<<7)|(1<<9)))
        {
            distance = transform.position.y -  hit.point.y;

        }
#endif
        int length = (int)distance;

        Vector2 topLeft = new Vector2(x - 0.5f, y + 0.5f);
        Vector2 topRight = new Vector2(x + 0.5f, y + 0.5f);
        Vector2 bottomLeft = new Vector2(x - 0.5f, y - 0.5f - length);
        Vector2 bottomRight = new Vector2(x + 0.5f, y - 0.5f - length);
        Color lineColor = Color.blue;
        Debug.DrawLine(topLeft, topRight, lineColor);
        Debug.DrawLine (bottomLeft, bottomRight, lineColor);
        Debug.DrawLine(bottomRight, topRight, lineColor);
        Debug.DrawLine(topLeft, bottomLeft, lineColor);
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
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            player = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour
{
    public int Length = 1;
    private BoxCollider2D _collider;
    public GameObject LadderSpriteObject;

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //float halfHeight = waterHeight / 2f;
        float halfWidth = 1 / 2f;

        Vector3 topLeft = transform.position + new Vector3(-halfWidth, Length, 0f);
        Vector3 topRight = transform.position + new Vector3(halfWidth, Length, 0f);
        Vector3 downLeft = transform.position + new Vector3(-halfWidth, 0, 0f);
        Vector3 downRight = transform.position + new Vector3(halfWidth, 0, 0f);


        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, downLeft);
        Gizmos.DrawLine(downRight, topRight);
        Gizmos.DrawLine(downRight, downLeft);

    }
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        
    }
    private void Start()
    {
        SpawnLadder();
    }
    public void SpawnLadder()
    {
      
        _collider.size = new Vector2(1, Length);
        _collider.offset = new Vector2(0,1/2f * Length);

        for (int i = 0; i < Length; i++)
        {
            GameObject go = Instantiate(LadderSpriteObject, transform);
            go.transform.position = transform.position + new Vector3(0, i + 0.5f, 0);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMgr mgr = collision.GetComponent<PlayerMgr>();
            mgr.moveController.isTouchLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMgr mgr = collision.GetComponent<PlayerMgr>();
            mgr.moveController.isTouchLadder = false;
        }
    }
}

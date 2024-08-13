using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMgr : MonoBehaviour
{
    public Camera _camera;
    public List<EnvironmentObject> _environmentObjects;
    private List<Vector3> OriginPosList = new List<Vector3>();
    private Vector3 Center;
    private void Awake()
    {
        Center = transform.position;
        for (int i = 0; i < _environmentObjects.Count; i++)
        {
            OriginPosList.Add(_environmentObjects[i].target.transform.position);
        }

    }
    private void Update()
    {
       

        for (int i = 0; i < _environmentObjects.Count; i++)
        {
            Vector3 delta = _camera.transform.position - Center;
            delta.z = 0;
            float s = _environmentObjects[i].MoveStrength;
            GameObject tar = _environmentObjects[i].target;
            tar.transform.position = OriginPosList[i] + delta * s;
        }


    }
}
[System.Serializable]
public class EnvironmentObject
{
    public GameObject target;
    public float MoveStrength;
}
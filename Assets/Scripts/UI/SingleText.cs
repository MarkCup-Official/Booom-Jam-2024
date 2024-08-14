using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SingleText : MonoBehaviour
{
   private Text text;
    private Vector3 offset;
   public Transform target;
    public float FadeTime;
    public bool isPersistent;
    private float FadeTimer;
    private Camera cam;
   
    public void Init(string TextContent,Transform target,Vector3 offset,float FadeTime)
    {
        if(text == null)
        text = GetComponent<Text>();
        if (cam == null)
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        FadeTimer = 0;
        this.FadeTime = FadeTime;
        this.target = target;
        this.offset = offset;
        text.text = TextContent;
        this.isPersistent = false;
    }
    public void Init(string TextContent, Transform target, Vector3 offset)
    {
        if (text == null)
            text = GetComponent<Text>();
        if (cam == null)
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        FadeTimer = 0;
        this.target = target;
        this.offset = offset;
        text.text = TextContent;
        this.isPersistent = true;
    }
    public void ChangeText(string content)
    {
        text.text = content;
    }
    private void Update()
    {
        if(!isPersistent)
        FadeTimer += Time.deltaTime;
        if (FadeTimer > FadeTime)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }

        if (target == null) return;
        
      
        Vector3 pos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = pos;
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void DisShow()
    {
        gameObject.SetActive(false);
    }
    public void Clean()
    {
        if(isPersistent)
        Destroy(gameObject);
    }
}

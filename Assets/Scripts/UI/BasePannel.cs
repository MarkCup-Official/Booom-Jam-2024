using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI
{
    [RequireComponent(typeof(Canvas))]
    /// <summary>
    /// ����UI�����صĽű�����
    /// </summary>
    public abstract class BasePannel : MonoBehaviour
    {
        protected Canvas canvas;
        public virtual void Init(PannelLayer layer)
        {
            canvas = GetComponent<Canvas>();
            ChangeSortingLayer(layer);
        }
        public abstract void OnShowUp();
        public abstract void OnUpdate();
        public abstract void OnClose();

        public virtual bool IsActive()
        {
            return gameObject.activeSelf;
        }

        
        public void ChangeSortingLayer(PannelLayer layer)
        {
            if (canvas == null) return;
            canvas.sortingOrder = (int)layer;
        }

    }
    
    public enum PannelLayer
    {
        
        GameUI,//Ѫ����ʾ����Ʒ��ʾ�����͵�UI
        InfoUI,
        MenuUI,


    }
}

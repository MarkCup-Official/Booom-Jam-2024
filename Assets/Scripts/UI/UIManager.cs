using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
/// <summary>
/// ��������UI
/// UI��Ҫ��ʼ��,init����Ϊ��ʼ������������Ĳ���ΪUI������
/// �����ص�UIԤ������Ҫ����Resources�ļ�����
/// </summary>
public class UIManager : BaseMonoManager<UIManager>
{
    public static string TextUIName = "Pannel/TextUI";
    protected override void Awake()
    {
        if (UIManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
       
        ShowUI(TextUIName, PannelLayer.GameUI);
    }
   
    //�Ѿ����ɵ�UI�б�

    private Dictionary<string, BasePannel> UIPannelsDic = new Dictionary<string, BasePannel>();

    public BasePannel GetUI(string key)
    {
        if (!UIPannelsDic.ContainsKey(key)) throw new System.NullReferenceException();
        return UIPannelsDic[key];
    }
    public BasePannel InitUI(string PannelAssetID, PannelLayer pannelLayer)
    {
        if (UIPannelsDic.ContainsKey(PannelAssetID)) throw new System.Exception("alredyInit");
        GameObject UIprefab = Resources.Load<GameObject>(PannelAssetID);
        BasePannel pannel = Instantiate(UIprefab, transform).GetComponent<BasePannel>();
        pannel.gameObject.SetActive(false);
        pannel.Init(pannelLayer);
        UIPannelsDic.Add(PannelAssetID,pannel);
        return pannel;
    }
    public BasePannel ShowUI(string PannelAssetID, PannelLayer pannelLayer)
    {
        //�������

        if (!UIPannelsDic.ContainsKey(PannelAssetID)) InitUI(PannelAssetID,pannelLayer);

        UIPannelsDic[PannelAssetID].OnShowUp();
        return UIPannelsDic[PannelAssetID];
    }
    public BasePannel ShowUI(string PannelAssetID)
    {
        //�������

        if (!UIPannelsDic.ContainsKey(PannelAssetID)) throw new System.NullReferenceException();
       
        UIPannelsDic[PannelAssetID].OnShowUp();
        return UIPannelsDic[PannelAssetID];
    }
    public BasePannel CloseUI(string PannelAssetID)
    {
        if (!UIPannelsDic.ContainsKey(PannelAssetID)) throw new System.NullReferenceException();
        if (UIPannelsDic[PannelAssetID].gameObject.activeSelf == false) return UIPannelsDic[PannelAssetID];

       
        UIPannelsDic[PannelAssetID].OnClose();
        return UIPannelsDic[PannelAssetID];
    }
    public BasePannel SwitchUI(string PannelAssetID)
    {
        if (!UIPannelsDic.ContainsKey(PannelAssetID)) throw new System.NullReferenceException();
        if (UIPannelsDic[PannelAssetID].gameObject.activeSelf == false)
        {
            ShowUI(PannelAssetID);
        }
        else
        {
            CloseUI(PannelAssetID);
        }
        return UIPannelsDic[PannelAssetID];
    }
    public void ClearAllUI()
    {
        foreach (var item in UIPannelsDic)
        {
            Destroy(item.Value.gameObject);
        }
        UIPannelsDic.Clear();
    }

   
}


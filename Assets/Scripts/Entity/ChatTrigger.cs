using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
   private ChatPannel chatPannel;
    [Multiline]
    public List<string> content;

    private void Start()
    {
        chatPannel = UIManager.Instance.GetUI(UIManager.ChatUIName) as ChatPannel;
    }
    public void StartPrint()
    {
        chatPannel.OnShowUp();
        chatPannel.Show(content);
    }
}

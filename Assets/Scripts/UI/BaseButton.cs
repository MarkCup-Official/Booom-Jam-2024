using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class BaseButton : MonoBehaviour
{
    public Text buttonText;
    private Button button;
    public void InitButton(ButtonData data)
    {
        button = GetComponent<Button>();
        buttonText.text = data.ButtonName;
        button.onClick.AddListener(() => { data.action?.Invoke(); });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MainTitle : MonoBehaviour
{

    public GameObject buttonPrefab;
    public Transform buttonParent;
    public ButtonGroup buttonGroup;

    private void Awake()
    {
        foreach (var item in buttonGroup.buttons)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, buttonParent);
            BaseButton button = null;
            if (buttonObject.TryGetComponent(out button))
            {
                button.InitButton(item);
            }
        }

    }
    public void StartGame()
    {
        AsyncOperation load =  SceneManager.LoadSceneAsync(1);
        load.completed += (AsyncOperation complete) =>
        {

        };
    }
    public void OpenSetting()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
[System.Serializable]
public class ButtonGroup
{
    public Sprite ReadySprite;
    public Sprite FocusSprite;
    public Sprite ClickSprite;
    public List<ButtonData> buttons = new List<ButtonData>();
}

[System.Serializable]
public class ButtonData
{
    public string ButtonName;
    public UnityEvent action;
}
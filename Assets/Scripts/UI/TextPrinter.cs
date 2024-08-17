using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class TextPrinter : MonoBehaviour
{
    public AudioClip printClip;
    public float volume;
    public int printCycle = 1;
    private int current;
    [Multiline]
    public List<string> content = new List<string>();
    public float printInterval = 0.2f;
    public float breakInterval = 1;
    private Text text;
    private bool isInGame;
    private int CurIndex;
    private bool isPrint;
    public UnityEvent OnPrintOver;
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        if (isInGame) return;

        isInGame = true;
        text = gameObject.GetComponent<Text>();
    }
    public bool Set(List<string> content)
    {
        if (isPrint)
        {
            this.content.AddRange(content);
            return false;
        }

        CurIndex = 0;
        this.content = content;
        return true;
    }
    [ContextMenu("StartPrint")]
    public void StartPrint()
    {
        if (!isInGame) return;

        StartCoroutine(PrintCourtine());

    }
    public IEnumerator PrintCourtine()
    {
        text.text = "";
        isPrint = true;
        while (CurIndex < content.Count)
        {

            for (int i = 0; i <= content[CurIndex].Length; i++)
            {
                text.text = content[CurIndex].Substring(0, i).Trim();
                current++;
                if (SoundManager.Instance && current >= printCycle)
                {
                    SoundManager.Instance.PlaySound(printClip, volume);
                    current = 0;
                }

                yield return new WaitForSeconds(printInterval);
            }

            yield return new WaitForSeconds(breakInterval);
            CurIndex++;
        }
        isPrint = false;
        OnPrintOver?.Invoke();
    }

}

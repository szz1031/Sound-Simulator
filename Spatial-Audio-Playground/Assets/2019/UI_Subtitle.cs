using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class UI_Subtitle : MonoBehaviour,ISingleCoroutine {
    public float F_Offset;
    public float F_TipsLastTime = 4f;
    class SubtitleItem
    {
        public float m_startTime;
        public string m_subtitle;
        public int m_text;
        public SubtitleItem(string subtitle,float startTime,int index,Text text)
        {
            m_text = index;
            text.text = subtitle;
            m_subtitle = subtitle;
            m_startTime = startTime;
            text.SetActivate(true);
        }
    }
    Image m_Background;
    Color m_StartBackgroundColor;
    Queue<SubtitleItem> m_subTitles = new Queue<SubtitleItem>();
    List<Text> m_texts = new List<Text>();
    int textIndex = 0;
    private void Awake()
    {
        if (transform.Find("Background"))
        {
            m_Background = transform.Find("Background").GetComponent<Image>();
            m_StartBackgroundColor = m_Background.color;
        }

        Text text= transform.Find("Text").GetComponent<Text>();
        
        m_texts.Add(text);
        for (int i = 0; i < 3; i++)
        {
            m_texts.Add(Instantiate(text, transform));
            m_texts[i + 1].name = "Text" + (i+1).ToString();
        }

        for (int i = 0; i < 4; i++)
            m_texts[i].SetActivate(false);

        Repositon();
    }

    private void Update()
    {
        if (m_subTitles.Count > 0)
        {
            int dequeCount = 0;
            foreach (SubtitleItem subtitle in m_subTitles)
            {
                if (Time.time - subtitle.m_startTime > F_TipsLastTime)
                {
                    m_texts[subtitle.m_text].SetActivate(false);
                    dequeCount++;
                }
            }

            if (dequeCount == 0)
                return;

            for (int i = 0; i < dequeCount; i++)
                m_subTitles.Dequeue();

            Repositon();
        }
    }

    public void AddSubtitle(string subtitle)
    {
        textIndex = (textIndex+1) % 4;
        Text text = m_texts[textIndex];
        if (m_subTitles.Count == 4)
        {
            m_subTitles.Dequeue();
        }
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(20, 225, 0);
        m_subTitles.Enqueue(new SubtitleItem(subtitle,Time.time,textIndex,text));
        Repositon();
    }
    void Repositon()
    {
        int index = 0;
        foreach (SubtitleItem subtitle in m_subTitles)
            m_texts[ subtitle.m_text].GetComponent<RectTransform>().anchoredPosition = new Vector3(20,index++* F_Offset, 0);

        if(m_Background)
        m_Background.color = m_subTitles.Count == 0 ? new Color(0, 0, 0, 0) : m_StartBackgroundColor;
    }
}

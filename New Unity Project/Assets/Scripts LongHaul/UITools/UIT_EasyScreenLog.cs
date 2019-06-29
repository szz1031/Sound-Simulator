using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIT_EasyScreenLog : SimpleSingletonMono<UIT_EasyScreenLog> {
    public int LogExistCount = 10;
    public int LogSaveCount = 30;
    Text UIText_Log;
    List<log> List_Log = new List<log>();
    struct log
    {
        public string logInfo;
        public string logTrace;
        public LogType logType;
    }
    protected override void Awake()
    {
        base.Awake();
        UIText_Log = this.GetComponent<Text>();
        UIText_Log.text = "";
    }
    private void OnEnable()
    {
        Application.logMessageReceived += OnLogReceived;
    }
    private void OnDisbable()
    {
        Application.logMessageReceived -= OnLogReceived;
    }
    void OnLogReceived(string info,string trace,LogType type)
    {
        log tempLog = new log();
        tempLog.logInfo = info;
        tempLog.logTrace = trace;
        tempLog.logType = type;
        List_Log.Add(tempLog);
        if (List_Log.Count > LogSaveCount)
        {
            List_Log.RemoveAt(0);
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        if(UIText_Log!=null)
        UIText_Log.text = "";
        int startIndex = 0;
        int listCount = List_Log.Count;
        if (listCount >= LogExistCount)
        {
            startIndex = listCount - LogExistCount;
        }
        for (int i=startIndex; i < listCount; i++)
        {
            if(UIText_Log!=null)
            UIText_Log.text += "<color=#"+GetUIColorParam(List_Log[i].logType)+">" + List_Log[i].logInfo+"</color>\n";
        }
    }
    string GetUIColorParam(LogType type)
    {
        string colorParam = "";
        switch (type)
        {
            case LogType.Log:
                colorParam = "00FF28";
                break;
            case LogType.Warning:
                colorParam = "FFA900";
                break;
            case LogType.Exception:
            case LogType.Error:
                colorParam = "FF0900";
                break;
            case LogType.Assert:
            default:
                colorParam = "00E5FF";
                break;
        }
        return colorParam;
    }
    public void Clear()
    {
        List_Log.Clear();
        UpdateUI();
    }
    //#region Test
    //int test=0;
    //private void TestUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        Debug.Log(test++);
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        Debug.LogWarning(test++);
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        Debug.LogError(test++);
    //    }
    //}
    //#endregion
}

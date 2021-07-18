using System;
using System.Collections.Generic;
#pragma warning disable 0649        //Warnning Closed Cause  Use Private Field To Protected Value Set By Reflection 
struct SLocaliztaion:TExcel.ISExcel     //for excel Uses Only
{
    private string key;
    private string value;
    public string GetKey
    {
        get

        {
            return key;
        }
    }
    public string GetValue
    {
        get
        {
            return value;
        }
    }
    public void InitOnValueSet()
    {
    }
}
public enum enum_LanguageRegion
{
    CN,
    EN,
}
public static class TLocalization 
{
    public static bool IsInit = false;
    public static event Action OnLocaleChanged;
    public static enum_LanguageRegion e_CurLocation { get; private set; }
    static Dictionary<string, string> CurLocalization = new Dictionary<string, string>();
    public static void SetRegion(enum_LanguageRegion location)
    {
        e_CurLocation = location;
        TExcel.Properties<SLocaliztaion>.Init("_"+e_CurLocation.ToString());
        CurLocalization.Clear();
        for (int i = 0; i < TExcel.Properties<SLocaliztaion>.Count; i++)
        {
            CurLocalization.Add(TExcel.Properties<SLocaliztaion>.PropertiesList[i].GetKey, TExcel.Properties<SLocaliztaion>.PropertiesList[i].GetValue);
        }
        if (OnLocaleChanged != null)
            OnLocaleChanged();
        IsInit = true;
    }
    public static string Localize(this string key)
    {
        if (CurLocalization.ContainsKey(key))
        {
            return CurLocalization[key.Replace("\\n", "\n")];
        }
        UnityEngine.Debug.LogWarning("Localization Key:(" + key + ") Not Found In Localization " + e_CurLocation.ToString());
        return key;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public static class TCommonUI
{
    public static void SetAlpha(this MaskableGraphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }
    public static void RaycastAll(Vector2 castPos)      //Bind UIT_EventTriggerListener To Items Need To Raycast By EventSystem
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = castPos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        for (int i = 0; i < results.Count; i++)
        {
            UIT_EventTriggerListener listener = results[i].gameObject.GetComponent<UIT_EventTriggerListener>();
            if (listener != null)
                listener.OnRaycast();
        }
    }
}
public static class TCommon
{
    public static void SetActivate(this MonoBehaviour behaviour, bool active)
    {
        if (behaviour.gameObject.activeSelf != active)
            SetActivate(behaviour.gameObject, active);
    }
    public static void SetActivate(this Transform tra, bool active)
    {
        SetActivate(tra.gameObject, active);
    }
    public static void SetActivate(this GameObject go, bool active)
    {
        if (go.activeSelf != active)
            go.SetActive(active);
    }
    public static void DestroyChildren(this Transform trans)
    {
        if (trans.childCount > 0)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                GameObject.Destroy(trans.GetChild(i).gameObject);
            }
        }
    }
    public static void SetChildLayer(this Transform trans, int layer)
    {
        foreach (Transform temp in trans.gameObject.GetComponentsInChildren<Transform>(true))
        {
            temp.gameObject.layer = layer;
        }
    }
    public static void SetTransformShow(Transform tra, bool active)
    {
        tra.localScale = active ? Vector3.one : Vector3.zero;
    }
    public static float GetXZDistance(Vector3 start, Vector3 end)
    {
        return new Vector2(start.x - end.x, start.z - end.z).magnitude;
    }
    public static Vector3 GetXZLookDirection(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 lookDirection = endPoint - startPoint;
        lookDirection.y = 0;
        lookDirection.Normalize();
        return lookDirection;
    }
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f; float g = bg / 255f; float b = bb / 255f; float a = cc / 255f;
        return new Color(r, g, b, a);
    }
    public static Color ColorAlpha(Color origin, float alpha)
    {
        return new Color(origin.r, origin.g, origin.b, alpha);
    }
    public static float GetAngle180(float angle)
    {
        if (angle > 180)
            angle -= 360;
        return angle;
    }
    public static float GetAngle(Vector3 first, Vector3 second, Vector3 up)
    {
        float angle = Vector3.Angle(first, second);
        angle *= Mathf.Sign(Vector3.Dot(up, Vector3.Cross(first, second)));
        return angle;
    }

    public static float GetAngleY(Vector3 first, Vector3 second, Vector3 up)
    {
        Vector3 newFirst = new Vector3(first.x, 0, first.z);
        Vector3 newSecond = new Vector3(second.x, 0, second.z);
        return GetAngle(newFirst, newSecond, up);
    }

    public static float GetIncludedAngle(float angle1, float angle2)
    {
        float angle = 0;
        if (angle1 >= 270 && angle2 < 90)
        {
            angle = (angle1 - (angle2 + 360)) % 180;
        }
        else if (angle1 <= 90 && angle2 >= 270)
        {
            angle = (angle1 + 360 - angle2) % 180;
        }
        else
        {
            angle = (angle1 - angle2);
            if (Mathf.Abs(angle) > 180)
            {
                angle -= 360;
            }
        }
        return angle;
    }
    public static Quaternion RandomRotation()
    {
        return Quaternion.Euler(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
    }
    public static Vector3 RandomPositon(Vector3 startPosition, float offset = .2f)
    {
        return startPosition + new Vector3(UnityEngine.Random.Range(-offset,offset), UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset));
    }
    public static Transform FindOrCreateNewTransform(this Transform parentTrans, string name)
    {
        Transform toTrans;
        toTrans = parentTrans.Find(name);
        if (toTrans == null)
        {
            toTrans = new GameObject().transform;
            toTrans.SetParent(parentTrans);
            toTrans.name = name;
        }
        return toTrans;
    }
    #region List/Array/Enum Traversal
    public static List<int> SplitIndexComma(this string toSplit)
    {
        List<int> indexes = new List<int>();
        string[] splitIndexes = toSplit.Split(',');
        for (int i = 0; i < splitIndexes.Length; i++)
        {
            indexes.Add(int.Parse(splitIndexes[i]));
        }
        return indexes;
    }
    public static T Find<T>(this T[,] array,Predicate<T> predicate) 
    {
        for (int i = 0; i < array.GetLength(0); i++)
            for (int j = 0; j < array.GetLength(1); j++)
                if (predicate(array[i, j])) return array[i, j];
            return default(T);
    }

    public static T RandomItem<T>(this List<T> randomList,System.Random randomSeed=null)
    {
        return randomList[randomSeed!=null?randomSeed.Next(randomList.Count):UnityEngine.Random.Range(0, randomList.Count)];
    }
    public static int RandomIndex<T>(this List<T> randomList,System.Random randomSeed=null)
    {
        return randomSeed!=null?randomSeed.Next(randomList.Count):UnityEngine.Random.Range(0, randomList.Count);
    }
    public static T RandomItem<T>(this T[] array, System.Random randomSeed = null)
    {
        return randomSeed != null ? array[randomSeed.Next(array.Length)] :array[UnityEngine.Random.Range(0, array.Length)];
    }
    public static T RandomItem<T>(this T[,] array, System.Random randomSeed = null)
    {
        return randomSeed != null ? array[randomSeed.Next(array.GetLength(0)),randomSeed.Next(array.GetLength(1))] : array[UnityEngine.Random.Range(0, array.GetLength(0)), UnityEngine.Random.Range(0, array.GetLength(1))];
    }
    public static void Traversal<T>(this List<T> list, Action<T> OnEachItem) where T : class
    {
        for (int i = 0; i < list.Count; i++)
        {
            OnEachItem(list[i]);
        }
    }
    public static void Traversal<T>(this List<T> list, Action<int, T> OnEachItem) where T : class
    {
        for (int i = 0; i < list.Count; i++)
            OnEachItem(i, list[i]);
    }
    public static void Traversal<T, Y>(this Dictionary<T, Y> dic, Action<T> OnEachKey) where T : class where Y : class
    {
        foreach (T temp in dic.Keys)
            OnEachKey(temp);
    }
    public static void Traversal<T, Y>(this Dictionary<T, Y> dic, Action<Y> OnEachValue) where T : class  where Y : class
    {
        foreach (Y temp in dic.Values)
            OnEachValue(temp);
    }
    public static void Traversal<T, Y>(this Dictionary<T, Y> dic, Action<T, Y> OnEachPair) 
    {
        foreach (T temp in dic.Keys)
            OnEachPair(temp, dic[temp]);
    }
    public static void Traversal<T>(this T[] array, Action<T> OnEachItem) where T : class
    {
        for (int i = 0; i < array.Length; i++)
            OnEachItem(array[i]);
    }
    public static void Traversal<T>(this T[,] array, Action<T> OnEachItem) where T:class
    {
        for (int i = 0; i < array.GetLength(0); i++)
            for (int j = 0; j < array.GetLength(1); j++)
                OnEachItem(array[i, j]);
    }
    public static void Traversal<T>(this T[] array, Action<T, int> OnEachItem) where T : class
    {
        for (int i = 0; i < array.Length; i++)
            OnEachItem(array[i], i);
    }
    public static void TraversalEnum<T>(Action<T, int> enumAction)    //Can't Constraint T to System.Enum?
    {
        if (!typeof(T).IsSubclassOf(typeof(Enum)))
        {
            Debug.LogError("Can't Traversal EnEnum Class!");
            return;
        }

        foreach (object temp in Enum.GetValues(typeof(T)))
        {
            if (temp.ToString() == "Invalid")
                continue;
            enumAction((T)temp, (int)temp);
        }
    }

    public static void TraversalEnum<T>(Action<T> enumAction)    //Can't Constraint T to System.Enum?
    {
        if (!typeof(T).IsSubclassOf(typeof(Enum)))
        {
            Debug.LogError("Can't Traversal EnEnum Class!");
            return;
        }

        foreach (object temp in Enum.GetValues(typeof(T)))
        {
            if (temp.ToString() == "Invalid")
                continue;
            enumAction((T)temp);
        }
    }

    public static string ToStringLog<T>(this List<T> tempList)
    {
        string target = "";
        for (int i = 0; i < tempList.Count; i++)
        {
            target += tempList[i].ToString();
            target += " ";
        }
        return target;
    }

    public static void TraversalRandom<T>(this List<T> list, System.Random seed=null, Func<T, bool> OnRandomItemStop=null)
    {
        int index = list.RandomIndex(seed);
        for (int i = 0; i < list.Count; i++)
        {
            if (OnRandomItemStop!=null&&OnRandomItemStop(list[index]))
                break;

            index++;
            if (index == list.Count)
                index = 0;
        }
    }
    #endregion
    public static T GetComponentNullable<T>(this Transform parent) where T : MonoBehaviour
    {
        if (parent == null)
            return null;
        return parent.GetComponent<T>();
    }
    public static void SortChildByNameIndex(Transform transform, bool higherUpper = true)
    {
        List<Transform> childList = new List<Transform>();
        List<int> childIndexList = new List<int>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i));
            childIndexList.Add(int.Parse(childList[i].gameObject.name));
        }
        childIndexList.Sort((a, b) => { return a <= b ? (higherUpper ? 1 : -1) : (higherUpper ? -1 : 1); });

        for (int i = 0; i < childList.Count; i++)
        {
            childList[i].SetSiblingIndex(childIndexList.FindIndex(p => p == int.Parse(childList[i].name)));
        }
    }

    public static string ToLogText<T, Y>(this Dictionary<T, Y> dic)
    {
        string target = "";
        foreach (T temp in dic.Keys)
        {
            target += temp.ToString() + "|" + dic[temp].ToString() + " ";
        }
        return target;
    }

    public static int Random(this RangeInt ir,System.Random seed = null)
    {
        return ir.start + (seed != null ? seed.Next(ir.start, ir.end + 1) : UnityEngine.Random.Range(0, ir.length + 1));
    }
    public static float Random(this RangeFloat ir, System.Random seed = null)
    {
        return ir.start + (seed != null ? seed.Next((int)(ir.start*1000), (int)(ir.end*1000))/100 : UnityEngine.Random.Range(0, ir.length ));
    }
}
#region Extra Structs
public struct RangeFloat
{
    public float start;
    public float length;
    public float end => start + length;
    public RangeFloat(float _start, float _length)
    {
        start = _start;
        length = _length;
    }
}
#endregion
public class TXmlPhrase : SingleTon<TXmlPhrase>
{
    Dictionary<Type, Func<object, string>> dic_valueToXmlData = new Dictionary<Type, Func<object, string>>();
    Dictionary<Type, Func<string, object>> dic_xmlDataToValue = new Dictionary<Type, Func<string, object>>();
    Dictionary<Type, Func<object>> dic_xmlDataDefault = new Dictionary<Type, Func<object>>();
    public TXmlPhrase()
    {
        dic_valueToXmlData.Add(typeof(int), (object target) => { return target.ToString(); });
        dic_xmlDataToValue.Add(typeof(int), (string xmlData) => { return int.Parse(xmlData); });
        dic_xmlDataDefault.Add(typeof(int), () => {  return -1; });
        dic_valueToXmlData.Add(typeof(long), (object target) => { return target.ToString(); });
        dic_xmlDataToValue.Add(typeof(long), (string xmlData) => { return long.Parse(xmlData); });
        dic_xmlDataDefault.Add(typeof(long), () => { return -1; });
        dic_valueToXmlData.Add(typeof(double), (object target) => { return target.ToString(); });
        dic_xmlDataToValue.Add(typeof(double), (string xmlData) => { return double.Parse(xmlData); });
        dic_xmlDataDefault.Add(typeof(double), () => { return -1; });
        dic_valueToXmlData.Add(typeof(float), (object target) => { return target.ToString(); });
        dic_xmlDataToValue.Add(typeof(float), (string xmlData) => { return float.Parse(xmlData); });
        dic_xmlDataDefault.Add(typeof(float), () => { return -1f; });
        dic_valueToXmlData.Add(typeof(string), (object target) => { return target as string; });
        dic_xmlDataToValue.Add(typeof(string), (string xmlData) => { return xmlData; });
        dic_xmlDataDefault.Add(typeof(string), () => { return "Invalid"; });
        dic_valueToXmlData.Add(typeof(bool),(object data) => { return (((bool)data  ? 1 : 0)).ToString(); });
        dic_xmlDataToValue.Add(typeof(bool), (string xmlData) => { return int.Parse(xmlData) == 1; });
        dic_xmlDataDefault.Add(typeof(bool), () => { return false; });
        dic_valueToXmlData.Add(typeof(RangeInt), (object data) => { return ((RangeInt)data).start.ToString() + "," + ((RangeInt)data).length.ToString(); });
        dic_xmlDataToValue.Add(typeof(RangeInt), (string xmlData) => { string[] split = xmlData.Split(','); return new RangeInt(int.Parse(split[0]),int.Parse(split[1])); });
        dic_xmlDataDefault.Add(typeof(RangeInt), () => { return new RangeInt(-1, 0); });
        dic_valueToXmlData.Add(typeof(RangeFloat), (object data) => { return ((RangeFloat)data).start.ToString() + "," + ((RangeFloat)data).length.ToString(); });
        dic_xmlDataToValue.Add(typeof(RangeFloat), (string xmlData) => { string[] split = xmlData.Split(','); return new RangeFloat(float.Parse(split[0]), float.Parse(split[1])); });
        dic_xmlDataDefault.Add(typeof(RangeFloat), () => { return new RangeFloat(-1, 0); });
    }
    public static TXmlPhrase Phrase
    {
        get
        {
            return Instance;
        }
    }
    public object GetDefault(Type type) =>dic_xmlDataDefault.ContainsKey(type)? dic_xmlDataDefault[type](): type.IsValueType ? Activator.CreateInstance(type) : null;
    public string this[Type type, object value]
    {
        get
        {
            StringBuilder sb_xmlData = new StringBuilder();
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type listType = type.GetGenericArguments()[0];
                    foreach (object obj in value as IEnumerable)
                    {
                        sb_xmlData.Append(ValueToXmlData(listType, obj));
                        sb_xmlData.Append(";");
                    }
                    sb_xmlData.Remove(sb_xmlData.Length - 1, 1);
                }
                else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type keyType = type.GetGenericArguments()[0];
                    Type valueType = type.GetGenericArguments()[1];
                    foreach (DictionaryEntry obj in (IDictionary)value)
                    {
                        sb_xmlData.Append(ValueToXmlData(keyType, obj.Key));
                        sb_xmlData.Append(":");
                        sb_xmlData.Append(ValueToXmlData(valueType, obj.Value));
                        sb_xmlData.Append(";");
                    }
                    sb_xmlData.Remove(sb_xmlData.Length - 1, 1);
                }
            }
            else
            {
                sb_xmlData.Append(ValueToXmlData(type, value));
            }
            return sb_xmlData.ToString();
        }
    }
    public object this[Type type, string xmlData]
    {
        get
        {
            object obj_target = null;
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type listType = type.GetGenericArguments()[0];
                    IList iList_Target = (IList)Activator.CreateInstance(type);
                    string[] as_split = xmlData.Split(';');
                    if (as_split.Length != 1 || as_split[0] != "")
                        for (int i = 0; i < as_split.Length; i++)
                        {
                            iList_Target.Add(XmlDataToValue(listType, as_split[i]));
                        }
                    obj_target = iList_Target;
                }
                else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type keyType = type.GetGenericArguments()[0];
                    Type valueType = type.GetGenericArguments()[1];
                    IDictionary iDic_Target = (IDictionary)Activator.CreateInstance(type);
                    string[] as_split = xmlData.Split(';');
                    if (as_split.Length != 1 || as_split[0] != "")
                        for (int i = 0; i < as_split.Length; i++)
                        {
                            string[] as_subSplit = as_split[i].Split(':');
                            iDic_Target.Add(XmlDataToValue(keyType, as_subSplit[0])
                                , XmlDataToValue(valueType, as_subSplit[1]));
                        }
                    obj_target = iDic_Target;
                }
            }
            else
            {
                obj_target = XmlDataToValue(type, xmlData);
            }
            return obj_target;
        }
    }
    string ValueToXmlData(Type type, object value)
    {
        if (!dic_valueToXmlData.ContainsKey(type))
            Debug.LogWarning("Xml Error Invlid Type:" + type.ToString() + " For Base Type To Phrase");
        return dic_valueToXmlData[type](value);
    }
    object XmlDataToValue(Type type, string xmlData)
    {
        if (!dic_xmlDataToValue.ContainsKey(type))
            Debug.LogWarning("Xml Error Invlid Type:" + type.ToString() + " For Xml Data To Phrase");
        return dic_xmlDataToValue[type](xmlData);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
//List Of INumerators Will Be Used
public static class TIEnumerators
{
    public static class UI
    {
        public static IEnumerator StartTypeWriter(Text text,string writerText,float duration)
        {
            float startTime = Time.time;
            StringBuilder m_stringBuilder = new StringBuilder(writerText);
            int length=m_stringBuilder.Length;
            for (; ;)
            {
                float timeParam = (Time.time - startTime) / duration;
                if (timeParam > 1)
                {
                    text.text = writerText;
                    yield break;
                }
                text.text = m_stringBuilder.ToString(0, (int)(length * timeParam));
                yield return null;
            }
            
        }
    }
    public static IEnumerator Tick(Action OnTick, float duration = -1)
    {
        WaitForSeconds seconds = duration == -1 ? null : new WaitForSeconds(duration);
        OnTick();
        for (; ; )
        {
            yield return seconds;
            OnTick();
        }
    }
    public static IEnumerator TickCount(Action OnTick, int totalTicks, float duration = -1)
    {
        WaitForSeconds seconds = duration == -1 ? null : new WaitForSeconds(duration);
        int count = 1;
        OnTick();
        for (; ; )
        {
            if (count >= totalTicks)
                yield break;
            yield return seconds;
            OnTick();
            count++;
        }
    }
    public static IEnumerator TickDelta(Action<float> OnTickDelta, float duration = -1)
    {
        float preTime = Time.time;
        WaitForSeconds seconds = duration == -1 ? null : new WaitForSeconds(duration);
        OnTickDelta(Time.time - preTime);
        for (; ; )
        {
            yield return seconds;
            OnTickDelta(Time.time - preTime);
            preTime = Time.time;
        }
    }
    public static IEnumerator TickTraversel<T>(Action<T> OnTick, List<T> list, float duration = -1)
    {
        int index = 0;
        WaitForSeconds seconds = duration == -1 ? null : new WaitForSeconds(duration);
        OnTick(list[index++]);
        for (; ; )
        {
            yield return seconds;
            OnTick(list[index++]);
            if (index == list.Count)
                yield break;
        }
    }
    public static IEnumerator ChangeValueTo(Action<float> OnValueChanged, float startValue, float endValue, float duration, Action OnFinished = null)
    {
        float startTime = Time.time;
        for (; ; )
        {
            float timeParam = (Time.time - startTime) / duration;
            if (timeParam > 1)
            {
                OnValueChanged(endValue);
                OnFinished?.Invoke();
                yield break;
            }
            else
            {
                OnValueChanged(startValue + (endValue - startValue) * timeParam);
            }
            yield return null;
        }
    }
    public static IEnumerator PauseDel(float pauseDuration, Action del)
    {
        yield return new WaitForSeconds(pauseDuration);
        del();
    }
    public static IEnumerator PauseDel<T>(float pauseDuration, T template, Action<T> del)
    {
        yield return new WaitForSeconds(pauseDuration);
        del(template);
    }
    public static IEnumerator RectTransformLerpTo(RectTransform rectTrans, Vector3 startPos, Vector3 endPos, float duration, Action OnFinished = null)
    {
        float startTime = Time.time;
        for (; ; )
        {
            if (rectTrans == null)
            {
                yield break;
            }

            float timeParam = (Time.time - startTime) / duration;
            if (timeParam > 1)
            {
                rectTrans.anchoredPosition = endPos;
                if (OnFinished != null)
                    OnFinished();
                yield break;
            }
            else
            {
                rectTrans.anchoredPosition = Vector3.Lerp(startPos, endPos, timeParam);
            }
            yield return null;
        }
    }
    public static IEnumerator RigidbodyMovePosition(Rigidbody rigid, Vector3 startPos, Vector3 endPos, float duration, Action OnFinished = null)
    {

        float startTime = Time.time;
        for (; ; )
        {
            if (rigid == null)
            {
                yield break;
            }

            float timeParam = (Time.time - startTime) / duration;
            if (timeParam > 1)
            {
                rigid.MovePosition(endPos);
                if (OnFinished != null)
                    OnFinished();
                yield break;
            }
            else
            {
                rigid.MovePosition(Vector3.Lerp(startPos, endPos, timeParam));
            }
            yield return null;
        }
    }
    public static IEnumerator TransformLerpTo(Transform lerpTrans, Vector3 startPos, Vector3 endPos, float duration, bool isLocal, Action OnFinished = null)
    {
        float startTime = Time.time;
        for (; ; )
        {
            if (lerpTrans == null)
            {
                yield break;
            }

            float timeParam = (Time.time - startTime) / duration;
            if (timeParam > 1)
            {
                if (OnFinished != null)
                    OnFinished();
                yield break;
            }
            else
            {
                if (isLocal)
                    lerpTrans.localPosition = Vector3.Lerp(startPos, endPos, timeParam);
                else
                    lerpTrans.position = Vector3.Lerp(startPos, endPos, timeParam);
            }
            yield return null;
        }
    }

}

//Interface For CoroutineManager
public interface ISingleCoroutine
{
}
public static class ISingleCoroutine_Extend
{
    public static void StartSingleCoroutine(this ISingleCoroutine target, int index, IEnumerator numerator)
    {
        if (index < 0)
            Debug.LogWarning(" Should Not Add Coroutine Index Which Below 0");

        int targetIndex = CoroutineManager.QuestForIndex(target, index);
        if (CoroutineManager.Dic_Coroutines.ContainsKey(targetIndex))
        {
            if (CoroutineManager.Dic_Coroutines[targetIndex] != null)
                CoroutineManager.Instance.StopCoroutine(CoroutineManager.Dic_Coroutines[targetIndex]);
        }
        else
        {
            CoroutineManager.Dic_Coroutines.Add(targetIndex, null);
        }


        CoroutineManager.Dic_Coroutines[targetIndex] = CoroutineManager.Instance.StartCoroutine(numerator);
    }
    public static void StopSingleCoroutine(this ISingleCoroutine target, int index = 0)
    {
        int targetIndex = CoroutineManager.QuestForIndex(target, index);
        if (CoroutineManager.Dic_Coroutines.ContainsKey(targetIndex) && CoroutineManager.Dic_Coroutines[targetIndex] != null)
            CoroutineManager.Instance.StopCoroutine(CoroutineManager.Dic_Coroutines[targetIndex]);
    }

    public static void StopAllSingleCoroutines(this ISingleCoroutine target)
    {
        int min=0, max = 0;
        CoroutineManager.QuestForRange(target,ref min,ref max);
        foreach (int index in CoroutineManager.Dic_Coroutines.Keys)
        {
            if (index >= min && index <= max)
                if (CoroutineManager.Dic_Coroutines.ContainsKey(index) && CoroutineManager.Dic_Coroutines[index] != null)
                    CoroutineManager.Instance.StopCoroutine(CoroutineManager.Dic_Coroutines[index]);
        }
    }

    public static void StopSingleCoroutines(this ISingleCoroutine target, params int[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
            StopSingleCoroutine(target, indexes[i]);
    }
}

//Main Coroutine Manager
class CoroutineManager : SingletonMono<CoroutineManager>
{
    internal static List<ISingleCoroutine> L_Target = new List<ISingleCoroutine>();
    internal static Dictionary<int, Coroutine> Dic_Coroutines = new Dictionary<int, Coroutine>();
    internal static void QuestForRange(ISingleCoroutine target, ref int min, ref int max)
    {
        min = L_Target.FindIndex(p => p == target) * 1000;
        max = min + 999;
    }
    internal static int QuestForIndex(ISingleCoroutine target, int index)
    {
        int targetIndex = 0;
        if (!L_Target.Contains(target))
            L_Target.Add(target);

        targetIndex += L_Target.FindIndex(p => p == target) * 1000;
        targetIndex += index;
        return targetIndex;
    }
}
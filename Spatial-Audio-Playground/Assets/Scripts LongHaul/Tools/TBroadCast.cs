using System;
using System.Collections.Generic;
#region LocalMessages
public class LocalMessage
{
    public enum enum_MessageType
    {
        Void,
        Template,
        TemplateDouble,
    }
    public virtual enum_MessageType e_type => enum_MessageType.Void;
    public Action ac_listener { get; private set; }
    public LocalMessage(Action listener)
    {
        ac_listener = listener;
    }
    public void Trigger()
    {
        ac_listener.Invoke();
    }

    public LocalMessage()
    {
    }
}
public class LocalMessage<T> : LocalMessage
{
    public override enum_MessageType e_type => enum_MessageType.Template;
    public new Action<T> ac_listener { get; private set; }
    public LocalMessage(Action<T> _listener)
    {
        ac_listener = _listener;
    }
    public void Trigger(T _object)
    {
        ac_listener(_object);

    }
}
public class LocalMessage<T, Y> : LocalMessage
{
    public override enum_MessageType e_type => enum_MessageType.TemplateDouble;
    public new Action<T, Y> ac_listener { get; private set; }
    public LocalMessage(Action<T, Y> _listener)
    {
        ac_listener = _listener;
    }
    public void Trigger(T _object1, Y _object2)
    {
        ac_listener(_object1, _object2);
    }
}
#endregion


public class TBroadCaster<TEnum>  {      //Message Center  Add / Remove / Trigger
    private static Dictionary<TEnum, List<LocalMessage>> dic_delegates = new Dictionary<TEnum, List<LocalMessage>>();
    public static void Init() {
        TCommon.TraversalEnum((TEnum temp)=> { dic_delegates.Add(temp, new List<LocalMessage>()); });
    }

    public static void Add(TEnum type, Action Listener)
    {
        if (dic_delegates.Count == 0)
            UnityEngine.Debug.LogError("Please Init Broadcaster Before Add");

        dic_delegates[type].Add(new LocalMessage(Listener));
    }
    public static void Remove(TEnum type, Action Listener)
    {
        LocalMessage target=null;
        foreach (LocalMessage mb in dic_delegates[type])
        {
            if (mb.e_type == LocalMessage.enum_MessageType.Void && mb.ac_listener == Listener)
            {
                target = mb;
                break;
            }
        }
        if(target!=null)
            dic_delegates[type].Remove(target);
    }
    public static void Trigger(TEnum type)
    {
        bool triggered = false;
        foreach (LocalMessage del in dic_delegates[type])
        {
            if (del.e_type == LocalMessage.enum_MessageType.Void)
            {
                triggered = true;
                del.Trigger();
            }
        }
        if(!triggered)
            UnityEngine.Debug.LogWarning("No Message Triggered By:" + type.ToString() );
    }

    public static void Add<T>(TEnum type, Action<T> Listener)
    {
        if (dic_delegates.Count == 0)
            UnityEngine.Debug.LogError("Please Init Broadcaster Before Add");

        dic_delegates[type].Add(new LocalMessage<T>(Listener));
    }
    public static void Remove<T>(TEnum type, Action<T> Listener)
    {
        LocalMessage<T> removeTarget = null;
        foreach (LocalMessage mb in dic_delegates[type])
        {
            if (mb.e_type == LocalMessage.enum_MessageType.Template)
                removeTarget = mb as LocalMessage<T>;
            if (removeTarget != null && removeTarget.ac_listener == Listener)
            {
                break;
            }
        }

        if (removeTarget!=null)
            dic_delegates[type].Remove(removeTarget);
    }
    public static void Trigger<T>(TEnum type, T template)
    {
        bool triggered = false;
        foreach (LocalMessage del in dic_delegates[type])
        {
            LocalMessage<T> target=null;
            if (del.e_type == LocalMessage.enum_MessageType.Template)
                target = del as LocalMessage<T>;

            if (target != null)
            {
                triggered = true;
                target.Trigger(template);
            }
        }

        if (!triggered)
            UnityEngine.Debug.LogWarning("No Message Triggered By:" + type.ToString() + "|" + template.ToString());
    }

    public static void Add<T,Y>(TEnum type, Action<T, Y> Listener)
    {
        if (dic_delegates.Count == 0)
            UnityEngine.Debug.LogError("Please Init Broadcaster Before Add");

        dic_delegates[type].Add(new LocalMessage<T,Y>(Listener));
    }
    public static void Remove<T,Y>(TEnum type, Action<T,Y> Listener)
    {
        LocalMessage<T,Y> removeTarget = null;
        foreach (LocalMessage mb in dic_delegates[type])
        {
            if (mb.e_type == LocalMessage.enum_MessageType.TemplateDouble)
                removeTarget = mb as LocalMessage<T,Y>;
            if (removeTarget != null && removeTarget.ac_listener == Listener)
            {
                break;
            }
        }

        if (removeTarget != null)
            dic_delegates[type].Remove(removeTarget);
    }
    public static void Trigger<T, Y>(TEnum type, T template1,Y template2)
    {
        bool triggered = false;
        foreach (LocalMessage del in dic_delegates[type])
        {
            LocalMessage<T,Y> target = null;
            if (del.e_type == LocalMessage.enum_MessageType.TemplateDouble)
                target = del as LocalMessage<T,Y>;

            if (target != null)
            {
                triggered = true;
                target.Trigger(template1, template2);
            }
        }

        if (!triggered)
            UnityEngine.Debug.LogWarning("No Message Triggered By:" + type.ToString() + "|" + template1.ToString() + "|" + template2.ToString());
    }
}

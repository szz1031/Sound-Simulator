using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public static class TReflection 
{
    public static List<Type> GetAllInheritClasses<T> ()
    {
        List<Type> classes= Assembly.GetExecutingAssembly().GetTypes().ToList();
        return classes.FindAll(p => p.IsSubclassOf(typeof(T)));
    }
    public static void InovokeAllMethod<T>(List<Type> classes,string methodName,T template) where T:class
    {
        foreach (Type t in classes)
        {
            MethodInfo method = t.GetMethod(methodName);
            if (method != null)
                method.Invoke(null,new object[] {template });
            else
                UnityEngine.Debug.LogError("Null Method Found From:"+t.ToString()+"."+methodName);
        }
    }
}

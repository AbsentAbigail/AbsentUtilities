using System;
using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[PublicAPI]
public static class ScriptableHelper
{
    public static T CreateScriptable<T>(string name, Action<T> modification = null) where T : ScriptableObject
    {
        var data = ScriptableObject.CreateInstance<T>();
        data.name = name;
        modification?.Invoke(data);
        return data;
    }
}
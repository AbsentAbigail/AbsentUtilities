﻿using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[PublicAPI]
public static class LogHelper
{
    public static void Log(string message)
    {
        var prefix = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Prefix;
        Debug.Log($"[{prefix}] {message}");
    }

    public static void Warn(string message)
    {
        var prefix = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Prefix;
        Debug.LogWarning($"[{prefix} Warning] {message}");
    }

    public static void Error(string message)
    {
        var prefix = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Prefix;
        Debug.LogError($"[{prefix} Error] {message}");
    }
}
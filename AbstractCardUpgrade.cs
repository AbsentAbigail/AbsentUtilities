﻿using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractCardUpgrade(
    string name,
    string title,
    string text,
    Pools pools = Pools.General,
    Action<CardUpgradeData> subscribe = null
)
{
    private Action<CardUpgradeData> _subscribe = subscribe ?? delegate { };

    public virtual CardUpgradeDataBuilder Builder()
    {
        return new CardUpgradeDataBuilder(AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod)
            .Create(name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage($"{name}.png")
            .WithTitle(title)
            .WithText(text)
            .WithPools(PoolsToStringArray(pools))
            .SubscribeToAfterAllBuildEvent(_subscribe.Invoke);
    }

    public static string[] PoolsToStringArray(Pools pool)
    {
        string[] pools = [];

        if (pool.HasFlag(Pools.General))
            pools = pools.AddToArray("GeneralCharmPool");

        if (pool.HasFlag(Pools.Snowdweller))
            pools = pools.AddToArray("BasicCharmPool");

        if (pool.HasFlag(Pools.Shademancer))
            pools = pools.AddToArray("MagicCharmPool");

        if (pool.HasFlag(Pools.Clunkmaster))
            pools = pools.AddToArray("ClunkCharmPool");

        return pools;
    }
}
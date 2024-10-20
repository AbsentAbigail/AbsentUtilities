using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[PublicAPI]
public class AbstractStatus<T>(
    string id,
    string text = null,
    bool canStack = true,
    bool canBoost = false,
    Action<T> subscribe = null) where T : StatusEffectData
{
    protected readonly bool CanBoost = canBoost;
    protected readonly bool CanStack = canStack;
    protected readonly string ID = id;
    protected readonly string Text = text;
    protected Action<T> Subscribe = subscribe ?? delegate { };
    protected AbsentUtils.ModInfo ModInfo;

    public virtual StatusEffectDataBuilder Builder()
    {
        ModInfo ??= AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        Debug.Log("Name: " + ID);
        var status = new StatusEffectDataBuilder(ModInfo.Mod)
            .Create<T>(ID)
            .WithStackable(CanStack)
            .WithCanBeBoosted(CanBoost)
            .SubscribeToAfterAllBuildEvent(d => Subscribe.Invoke(d as T));

        if (Text != null)
            status.WithText(Text);

        return status;
    }
}
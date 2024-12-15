using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using static StatusEffectApplyX;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractApplyXStatus<T>(
    string id,
    string text = null,
    bool canStack = true,
    bool canBoost = false,
    string effectToApply = "Snow",
    ApplyToFlags applyToFlags = ApplyToFlags.Self,
    Action<T> subscribe = null)
    : AbstractStatus<T>(id, text, canStack, canBoost, subscribe) where T : StatusEffectApplyX
{
    public override StatusEffectDataBuilder Builder()
    {
        ModInfo ??= AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        return base.Builder()
            .SubscribeToAfterAllBuildEvent(data =>
            {
                var status = (T)data;

                if (!effectToApply.IsNullOrWhitespace())
                    status.effectToApply = AbsentUtils.GetStatus(effectToApply, ModInfo);
                status.applyToFlags = applyToFlags;
            });
    }
}
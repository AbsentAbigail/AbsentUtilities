using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractCompanion(
    string id,
    string title,
    int? health = null,
    int? attack = null,
    int counter = 0,
    Pools pools = Pools.General,
    Action<CardData> subscribe = null,
    bool altSprite = false
) : AbstractCard(id, title, health, 0, attack, counter, pools, subscribe, altSprite)
{
    public virtual CardDataBuilder Builder()
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        return new CardDataBuilder(modInfo.Mod)
            .CreateUnit(ID, Title)
            .SetStats(Health, Attack, Counter)
            .SetAddressableSprites(ID, AltSprite, modInfo)
            .WithPools(UnitPools(Pools))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(Subscribe.Invoke);
    }
}
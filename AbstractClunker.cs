using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractClunker(
    string id,
    string title,
    int scrap = 1,
    int? attack = null,
    int counter = 0,
    Pools pools = Pools.General,
    Action<CardData> subscribe = null,
    bool altSprite = false) : AbstractCard(id, title, null, scrap, attack, counter, pools, subscribe, altSprite)
{
    public virtual CardDataBuilder Builder()
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());

        return new CardDataBuilder(modInfo.Mod)
            .CreateUnit(ID, Title)
            .WithCardType("Clunker")
            .SetStats(null, Attack, Counter)
            .SetStartWithEffect(AbsentUtils.SStack("Scrap", Scrap, modInfo))
            .SetAddressableSprites(ID, AltSprite, modInfo)
            .WithPools(ItemPools(Pools))
            .SubscribeToAfterAllBuildEvent(Subscribe.Invoke);
    }
}
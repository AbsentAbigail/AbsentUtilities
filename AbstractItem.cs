﻿using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractItem(
    string id,
    string title,
    int? attack = null,
    bool needsTarget = false,
    Pools pools = Pools.General,
    int shopPrice = 50,
    bool playOnHand = true,
    Action<CardData> subscribe = null,
    bool altSprite = false
) : AbstractCard(id, title, null, 0, attack, 0, pools, subscribe, altSprite)
{
    public virtual CardDataBuilder Builder()
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        return Builder(new CardDataBuilder(modInfo.Mod)
            .CreateItem(ID, Title)
            .SetDamage(Attack)
            .NeedsTarget(needsTarget)
            .WithValue(shopPrice)
            .WithPools(ItemPools(Pools))
            .CanPlayOnHand(playOnHand), modInfo);
    }
}
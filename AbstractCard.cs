using System;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractCard(
    string id,
    string title,
    int? health = null,
    int scrap = 0,
    int? attack = null,
    int counter = 0,
    Pools pools = Pools.General,
    Action<CardData> subscribe = null,
    bool altSprite = false
)
{
    protected readonly int? Attack = attack;
    protected readonly int Counter = counter;
    protected readonly int? Health = health;
    protected readonly Pools Pools = pools;
    protected readonly int Scrap = scrap;
    protected bool AltSprite = altSprite;
    protected string ID = id;
    public Action<CardData> Subscribe = subscribe ?? delegate { };
    protected string Title = title;
    public virtual string FlavourText => null;
    protected virtual string IdleAnimation => null;
    protected virtual string BloodProfile => null;

    public static string CardTag(string name)
    {
        return $"<card={AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod.GUID}.{name}>";
    }

    public static string[] UnitPools(Pools pool)
    {
        string[] pools = [];

        if (pool.HasFlag(Pools.General))
            pools = pools.AddToArray("GeneralUnitPool");

        if (pool.HasFlag(Pools.Snowdweller))
            pools = pools.AddToArray("BasicUnitPool");

        if (pool.HasFlag(Pools.Shademancer))
            pools = pools.AddToArray("MagicUnitPool");

        if (pool.HasFlag(Pools.Clunkmaster))
            pools = pools.AddToArray("ClunkUnitPool");

        return pools;
    }

    public static string[] ItemPools(Pools pool)
    {
        string[] pools = [];

        if (pool.HasFlag(Pools.General))
            pools = pools.AddToArray("GeneralItemPool");

        if (pool.HasFlag(Pools.Snowdweller))
            pools = pools.AddToArray("BasicItemPool");

        if (pool.HasFlag(Pools.Shademancer))
            pools = pools.AddToArray("MagicItemPool");

        if (pool.HasFlag(Pools.Clunkmaster))
            pools = pools.AddToArray("ClunkItemPool");

        return pools;
    }

    protected CardDataBuilder Builder(CardDataBuilder builder, AbsentUtils.ModInfo modInfo)
    {
        builder
            .SetAddressableSprites(ID, AltSprite, modInfo)
            .SubscribeToAfterAllBuildEvent(Subscribe.Invoke);
        if (FlavourText != null)
            builder.WithFlavour(FlavourText);
        if (IdleAnimation != null)
            builder.WithIdleAnimationProfile(IdleAnimation);
        if (BloodProfile != null)
            builder.WithBloodProfile(BloodProfile);
        
        return builder;
    }
}
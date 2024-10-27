using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public static class CardHelper
{
    public static string CardTag(string name)
    {
        return $"<card={AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod.GUID}.{name}>";
    }

    public static CardDataBuilder DropsBling(this CardDataBuilder builder, int amount)
    {
        return builder.WithValue(amount * 36);
    }

    public static CardDataBuilder SetAddressableSprites(this CardDataBuilder builder, string id,
        bool altSprite, AbsentUtils.ModInfo modInfo = null)
    {
        modInfo ??= AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        
        var spriteName = altSprite && modInfo.HasAltArt ? "Alt" + id : id;
        var spriteNameBg = spriteName + "BG";
        
        var sprites = modInfo.Sprites;
        if (sprites is null)
            return builder.SetSprites(spriteName + ".png", spriteName + "BG.png");
        
        var main = sprites.GetSprite(spriteName);
        var bg = sprites.GetSprite(spriteNameBg);

        return builder.SetSprites(
            main ?? modInfo.Mod.ImagePath(spriteName + ".png").ToSprite(),
            bg ?? modInfo.Mod.ImagePath(spriteNameBg + ".png").ToSprite()
        );
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
}
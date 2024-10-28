using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public static class CardUpgradeExtensions
{
    public static CardUpgradeDataBuilder SetAddressableSprites(this CardUpgradeDataBuilder builder, string id,
        bool altSprite, AbsentUtils.ModInfo modInfo = null)
    {
        modInfo ??= AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());

        var spriteName = altSprite && modInfo.HasAltArt ? "Alt" + id : id;

        var sprites = modInfo.Sprites;
        if (sprites is null)
            return builder.WithImage(spriteName + ".png");

        var main = sprites.GetSprite(spriteName);

        return builder.WithImage(
            main ?? modInfo.Mod.ImagePath(spriteName + ".png").ToSprite()
        );
    }
}
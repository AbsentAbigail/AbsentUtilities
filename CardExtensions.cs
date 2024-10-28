using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public static class CardExtensions
{
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
}
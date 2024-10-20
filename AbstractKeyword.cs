using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractKeyword(string name, string title, string description)
{
    public static Color Orange = Color(255, 202, 87);
    public static Color White = Color(255, 255, 255);
    public static Color Gray = Color(166, 166, 166);

    public virtual KeywordDataBuilder Builder()
    {
        return new KeywordDataBuilder(AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod)
            .Create(name)
            .WithTitle(title)
            .WithTitleColour(Orange)
            .WithShowName(true)
            .WithDescription(description)
            .WithBodyColour(White)
            .WithNoteColour(Gray);
    }

    public static Color Color(int r, int g, int b)
    {
        Color color = new(
            r / 255F,
            g / 255F,
            b / 255F
        );

        return color;
    }

    public static string GetTag(string name)
    {
        return $"<keyword={AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod.GUID}.{name}>";
    }
}
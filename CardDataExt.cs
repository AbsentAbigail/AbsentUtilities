using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public static class CardDataExt
{
    public static void StartWithEffects(this CardData cardData, params Stack[] statusEffects)
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        cardData.startWithEffects =
        [
            .. cardData.startWithEffects,
            .. statusEffects.Select(s => AbsentUtils.SStack(s.ID, s.Amount, modInfo))
        ];
    }

    public static void AttackEffects(this CardData cardData, params Stack[] statusEffects)
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        cardData.attackEffects =
        [
            .. cardData.startWithEffects,
            .. statusEffects.Select(s => AbsentUtils.SStack(s.ID, s.Amount, modInfo))
        ];
    }

    public static void Traits(this CardData cardData, params Stack[] traits)
    {
        var modInfo = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        cardData.attackEffects =
        [
            .. cardData.startWithEffects,
            .. traits.Select(s => AbsentUtils.SStack(s.ID, s.Amount, modInfo))
        ];
    }
}

[PublicAPI]
public class Stack(string id, int amount)
{
    public readonly int Amount = amount;
    public readonly string ID = id;
}
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[UsedImplicitly]
public class LeaderHelper
{
    public static CardScript GiveUpgrade(string name = "Crown")
    {
        return ScriptableHelper.CreateScriptable<CardScriptGiveUpgrade>(
            $"Give {name}",
            script => script.upgradeData =
                AbsentUtils.TryGet<CardUpgradeData>(name, AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()))
        );
    }


    public static CardScript AddRandomHealth(int min, int max)
    {
        return ScriptableHelper.CreateScriptable<CardScriptAddRandomHealth>(
            $"Add Random Health Between {min} And {max}",
            script => script.healthRange = new Vector2Int(min, max)
        );
    }

    public static CardScript AddRandomDamage(int min, int max)
    {
        return ScriptableHelper.CreateScriptable<CardScriptAddRandomDamage>(
            $"Add Random Damage Between {min} And {max}",
            script => script.damageRange = new Vector2Int(min, max)
        );
    }

    public static CardScript AddRandomCounter(int min, int max)
    {
        return ScriptableHelper.CreateScriptable<CardScriptAddRandomCounter>(
            $"Add Random Counter Between {min} And {max}",
            script => script.counterRange = new Vector2Int(min, max)
        );
    }
}
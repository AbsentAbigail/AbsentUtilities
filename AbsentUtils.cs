using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.U2D;

namespace AbsentUtilities;

[PublicAPI]
public static class AbsentUtils
{
    private static Dictionary<Assembly, ModInfo> _modInfos = new();

    public static string PrefixGuid(string name, ModInfo modInfo = null)
    {
        return $"{(modInfo ?? GetModInfo(Assembly.GetCallingAssembly())).Mod.GUID}.{name}";
    }
    
    public static T TryGetOrNull<T>(string name, ModInfo mod = null) where T : DataFile
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());

        T data;
        if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
            data = mod.Mod.Get<StatusEffectData>(name) as T;
        else
            data = mod.Mod.Get<T>(name);

        return data;
    }

    public static T TryGet<T>(string name, ModInfo mod = null) where T : DataFile
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());

        var data = TryGetOrNull<T>(name, mod);

        return data ??
               throw new Exception(
                   $"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, mod.Mod)}]");
    }

    public static void UnloadFromClasses(ModInfo mod = null)
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());

        var tribes = AddressableLoader.GetGroup<ClassData>("ClassData");
        foreach (var pool in from tribe in tribes
                 where tribe != null && tribe.rewardPools != null
                 from pool in tribe.rewardPools
                 where pool != null
                 select pool) pool.list.RemoveAllWhere(item => item == null || item.ModAdded == mod.Mod);
    }

    public static CardData GetCard(string name, ModInfo mod = null)
    {
        return TryGet<CardData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static CardUpgradeData GetCardUpgrade(string name, ModInfo mod = null)
    {
        return TryGet<CardUpgradeData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static StatusEffectData GetStatus(string name, ModInfo mod = null)
    {
        return TryGet<StatusEffectData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static T GetStatusOf<T>(string name, ModInfo mod = null) where T : StatusEffectData
    {
        return TryGet<T>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static TraitData GetTrait(string name, ModInfo mod = null)
    {
        return TryGet<TraitData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static KeywordData GetKeyword(string name, ModInfo mod = null)
    {
        return TryGet<KeywordData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static ClassData GetTribe(string name, ModInfo mod = null)
    {
        return TryGet<ClassData>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }

    public static CardType GetCardType(string name, ModInfo mod = null)
    {
        return TryGet<CardType>(name, mod ?? GetModInfo(Assembly.GetCallingAssembly()));
    }
    
    public static CardData.StatusEffectStacks SStack(string name, int amount = 1, ModInfo mod = null)
    {
        return new CardData.StatusEffectStacks(
            GetStatus(name, mod ?? GetModInfo(Assembly.GetCallingAssembly())), amount);
    }

    public static CardData.TraitStacks TStack(string name, int amount = 1, ModInfo mod = null)
    {
        return new CardData.TraitStacks(
            GetTrait(name, mod ?? GetModInfo(Assembly.GetCallingAssembly())), amount
        );
    }

    public static StatusEffectDataBuilder StatusCopy(string oldName, string newName, ModInfo mod = null)
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());
        var data = GetStatus(oldName, mod).InstantiateKeepName();
        data.name = mod.Mod.GUID + "." + newName;
        var builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
        builder.Mod = mod.Mod;
        return builder;
    }

    public static CardDataBuilder CardCopy(string oldName, string newName, ModInfo mod = null)
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());
        var data = GetCard(oldName, mod).InstantiateKeepName();
        data.name = mod.Mod.GUID + "." + newName;
        var builder = data.Edit<CardData, CardDataBuilder>();
        builder.Mod = mod.Mod;
        return builder;
    }

    public static ClassDataBuilder TribeCopy(string oldName, string newName, ModInfo mod = null)
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());
        var data = GetTribe(oldName, mod).InstantiateKeepName();
        data.name = mod.Mod.GUID + "." + newName;
        var builder = data.Edit<ClassData, ClassDataBuilder>();
        builder.Mod = mod.Mod;
        return builder;
    }

    public static T[] StringsToDataFiles<T>(params string[] names) where T : DataFile
    {
        var mod = GetModInfo(Assembly.GetCallingAssembly());
        return names.Select((s) => TryGet<T>(s, mod)).ToArray();  
    }

    public static T[] RemoveNulls<T>(T[] data, ModInfo mod = null) where T : DataFile
    {
        mod ??= GetModInfo(Assembly.GetCallingAssembly());
        var list = data.ToList();
        list.RemoveAll(x => x == null || x.ModAdded == mod.Mod);
        return list.ToArray();
    }
    
    public static ModInfo GetModInfo(Assembly assembly = null)
    {
        return _modInfos.GetValueSafe(assembly ?? Assembly.GetCallingAssembly());
    }

    public static bool AddModInfo(ModInfo modInfo)
    {
        var assembly = Assembly.GetCallingAssembly();
        if (_modInfos.ContainsKey(assembly))
        {
            Debug.Log($"Element with Key [{assembly}] already exists. Skipping");
            return false;
        }

        _modInfos.Add(Assembly.GetCallingAssembly(), modInfo);
        return true;
    }

    [PublicAPI]
    public class ModInfo
    {
        public bool HasAltArt;
        public WildfrostMod Mod;
        public string Prefix;
        public SpriteAtlas Sprites;

        public void SetSprites(SpriteAtlas sprite)
        {
            Sprites = sprite;
            Debug.Log(Mod.GUID + " Sets sprites to " + Sprites);
        }
    }
}
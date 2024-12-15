using JetBrains.Annotations;
using UnityEngine;

namespace AbsentUtilities;

[UsedImplicitly]
public class TribeHelper
{
    public static RewardPool CreateRewardPool(string name, string type, DataFile[] list)
    {
        var pool = ScriptableObject.CreateInstance<RewardPool>();
        pool.name = name;
        pool.type = type;
        pool.list = list.ToList();
        return pool;
    }
}
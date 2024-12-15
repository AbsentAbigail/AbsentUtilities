using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using HarmonyLib.Tools;
using JetBrains.Annotations;

namespace AbsentUtilities;

[UsedImplicitly]
public class Mod : WildfrostMod
{
    public Mod(string baseDirectory) : base(baseDirectory)
    {
        AbsentUtils.AddModInfo(new AbsentUtils.ModInfo { Mod = this, Prefix = "UTILS ERROR" });

        HarmonyInstance.PatchAll(typeof(PatchHarmony));
    }

    public override string GUID => "absentabigail.wildfrost.absentutils";
    public override string[] Depends => [];
    public override string Title => "Absent Utils";
    public override string Description => "Utility Library by AbsentAbigail\n" +
                                          "Provides shared code for mods\n" +
                                          "\n" +
                                          "\n" +
                                          "Source code: https://github.com/AbsentAbigail/AbsentUtilities";
    
    [HarmonyPatch(typeof(DebugLoggerTextWriter), "WriteLine")]
    private class PatchHarmony
    {
        [UsedImplicitly]
        private static bool Prefix()
        {
            Postfix();
            return false;
        }

        private static void Postfix()
        {
            Logger.ChannelFilter = Logger.LogChannel.None | Logger.LogChannel.Warn | Logger.LogChannel.Error;
        }
    }
}
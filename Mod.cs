using Deadpan.Enums.Engine.Components.Modding;

namespace AbsentUtilities;

public class Mod : WildfrostMod
{
    public Mod(string baseDirectory) : base(baseDirectory)
    {
        AbsentUtils.AddModInfo(new AbsentUtils.ModInfo { Mod = this, Prefix = "UTILS ERROR" });
    }

    public override string GUID => AbsentUtils.ID;
    public override string[] Depends => [];
    public override string Title => "Absent Utils";
    public override string Description => "Modding Toolkit";
}
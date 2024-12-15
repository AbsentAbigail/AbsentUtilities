using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractTrait(string id, string keyword = null, params string[] effects)
{
    protected readonly string[] Effects = effects;
    protected readonly string ID = id;
    protected readonly string Keyword = keyword;

    public virtual TraitDataBuilder Builder()
    {
        var mod = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly());
        return new TraitDataBuilder(mod.Mod)
            .Create(ID)
            .SubscribeToAfterAllBuildEvent(trait =>
            {
                trait.effects = Effects.ToList().Select(e => AbsentUtils.GetStatus(e, mod)).ToArray();
                trait.keyword = AbsentUtils.GetKeyword(Keyword, mod);
            });
    }
}
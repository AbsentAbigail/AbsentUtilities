using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public enum Pools
{
    None = 0,
    General = 1,
    Snowdweller = 2,
    Shademancer = 4,
    Clunkmaster = 8
}
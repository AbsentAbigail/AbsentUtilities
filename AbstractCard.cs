using System;
using JetBrains.Annotations;

namespace AbsentUtilities;

[PublicAPI]
public abstract class AbstractCard(
    string id,
    string title,
    int? health = null,
    int scrap = 0,
    int? attack = null,
    int counter = 0,
    Pools pools = Pools.General,
    Action<CardData> subscribe = null,
    bool altSprite = false
)
{
    protected readonly int? Attack = attack;
    protected readonly int Counter = counter;
    protected readonly int? Health = health;
    protected readonly Pools Pools = pools;
    protected readonly int Scrap = scrap;
    protected bool AltSprite = altSprite;
    protected string ID = id;
    protected Action<CardData> Subscribe = subscribe ?? delegate { };
    protected string Title = title;
}
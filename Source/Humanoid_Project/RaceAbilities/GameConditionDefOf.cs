using RimWorld;
using Verse;

namespace RaceAbilities;

[DefOf]
public static class GameConditionDefOf
{
    public static GameConditionDef RunicStorm;

    static GameConditionDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
    }
}
using RimWorld;
using Verse;

namespace RaceWeapons;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef LaserMoteWorker;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}
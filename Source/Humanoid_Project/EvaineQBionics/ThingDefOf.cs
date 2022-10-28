using RimWorld;
using Verse;

namespace EvaineQBionics;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef Filth_BlueVomit;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}
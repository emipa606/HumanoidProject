using RimWorld;
using Verse;

namespace EvaineQBionics;

[DefOf]
public static class EffecterDefOf
{
    public static EffecterDef Blue_Vomit;

    public static EffecterDef BerserkStyle;

    static EffecterDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(EffecterDefOf));
    }
}
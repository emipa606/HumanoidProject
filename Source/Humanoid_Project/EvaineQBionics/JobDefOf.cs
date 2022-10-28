using RimWorld;
using Verse;

namespace EvaineQBionics;

[DefOf]
public static class JobDefOf
{
    public static JobDef BlueVomit;

    static JobDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
    }
}
using RimWorld;
using Verse;

namespace RaceAbilities;

[DefOf]
public static class DamageDefOf
{
    public static DamageDef BlueFire;

    static DamageDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
    }
}
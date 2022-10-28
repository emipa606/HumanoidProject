using RimWorld;
using Verse;

namespace HumanAbilities;

[DefOf]
public static class DamageDefOf
{
    public static DamageDef RedFire;

    static DamageDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
    }
}
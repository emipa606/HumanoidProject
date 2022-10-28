using RimWorld;
using Verse;

namespace HumanUpgraded;

[DefOf]
public static class DamageDefOf
{
    public static DamageDef GreenFire;

    static DamageDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
    }
}
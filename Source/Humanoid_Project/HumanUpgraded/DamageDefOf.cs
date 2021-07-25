using RimWorld;
using Verse;

namespace HumanUpgraded
{
    // Token: 0x0200000B RID: 11
    [DefOf]
    public static class DamageDefOf
    {
        // Token: 0x04000010 RID: 16
        public static DamageDef GreenFire;

        // Token: 0x06000022 RID: 34 RVA: 0x00002C68 File Offset: 0x00000E68
        static DamageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
        }
    }
}
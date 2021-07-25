using RimWorld;
using Verse;

namespace HumanAbilities
{
    // Token: 0x02000003 RID: 3
    [DefOf]
    public static class DamageDefOf
    {
        // Token: 0x04000001 RID: 1
        public static DamageDef RedFire;

        // Token: 0x06000004 RID: 4 RVA: 0x000021D0 File Offset: 0x000003D0
        static DamageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
        }
    }
}
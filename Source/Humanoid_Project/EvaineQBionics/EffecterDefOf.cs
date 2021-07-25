using RimWorld;
using Verse;

namespace EvaineQBionics
{
    // Token: 0x02000028 RID: 40
    [DefOf]
    public static class EffecterDefOf
    {
        // Token: 0x04000040 RID: 64
        public static EffecterDef Blue_Vomit;

        // Token: 0x04000041 RID: 65
        public static EffecterDef BerserkStyle;

        // Token: 0x06000082 RID: 130 RVA: 0x000050C5 File Offset: 0x000032C5
        static EffecterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EffecterDefOf));
        }
    }
}
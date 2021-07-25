using Verse;

namespace HediffSpecial
{
    // Token: 0x0200001E RID: 30
    public static class ExtensionUtility
    {
        // Token: 0x06000060 RID: 96 RVA: 0x0000423C File Offset: 0x0000243C
        public static T TryGetModExtension<T>(this Def def) where T : DefModExtension
        {
            var result = def.HasModExtension<T>() ? def.GetModExtension<T>() : default;

            return result;
        }
    }
}
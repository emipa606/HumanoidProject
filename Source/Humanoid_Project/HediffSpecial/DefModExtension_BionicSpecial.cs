using Verse;

namespace HediffSpecial
{
    // Token: 0x02000017 RID: 23
    public class DefModExtension_BionicSpecial : DefModExtension
    {
        // Token: 0x0400002B RID: 43
        public HediffDef autoHealHediff;

        // Token: 0x0400002A RID: 42
        public HediffDef curedBodyPart;

        // Token: 0x04000028 RID: 40
        public string growthText = "Regrowing: ";

        // Token: 0x04000027 RID: 39
        public int growthTicks = 1000;

        // Token: 0x04000025 RID: 37
        public int healTicks = 1000;

        // Token: 0x04000029 RID: 41
        public HediffDef protoBodyPart;

        // Token: 0x04000026 RID: 38
        public bool regrowParts = true;
    }
}
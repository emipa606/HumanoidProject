using Verse;

namespace HediffSpecial;

public class DefModExtension_BionicSpecial : DefModExtension
{
    public HediffDef autoHealHediff;

    public HediffDef curedBodyPart;

    public string growthText = "Regrowing: ";

    public int growthTicks = 1000;

    public int healTicks = 1000;

    public HediffDef protoBodyPart;

    public bool regrowParts = true;
}
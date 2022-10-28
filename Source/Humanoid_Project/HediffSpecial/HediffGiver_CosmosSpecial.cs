using Verse;

namespace HediffSpecial;

public class HediffGiver_CosmosSpecial : HediffGiver
{
    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
        TryApply(pawn);
    }
}
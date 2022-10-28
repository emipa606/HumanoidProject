using System.Text;
using RimWorld;
using Verse;

namespace HediffSpecial;

public class Hediff_RegrowinBodyPart : Hediff_AddedPart
{
    public override bool ShouldRemove => Severity >= def.maxSeverity;

    public override string TipStringExtra
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(base.TipStringExtra);
            stringBuilder.AppendLine("Efficiency".Translate() + ": " +
                                     def.addedPartProps.partEfficiency.ToStringPercent());
            stringBuilder.AppendLine(
                pawn.health.hediffSet
                    .GetFirstHediffOfDef(def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff).def
                    .GetModExtension<DefModExtension_BionicSpecial>().growthText + Severity.ToStringPercent());
            return stringBuilder.ToString();
        }
    }


    public override void PostRemoved()
    {
        base.PostRemoved();
        if (Severity >= 1f && pawn.health.hediffSet
                .GetFirstHediffOfDef(def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff).def
                .TryGetModExtension<DefModExtension_BionicSpecial>().curedBodyPart != null)
        {
            pawn.ReplaceHediffFromBodypart(Part, HediffDefOf.MissingBodyPart,
                pawn.health.hediffSet
                    .GetFirstHediffOfDef(def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff).def
                    .GetModExtension<DefModExtension_BionicSpecial>().curedBodyPart);
            return;
        }

        if (Severity >= 1f)
        {
            pawn.ReplaceHediffFromBodypart(Part, HediffDefOf.MissingBodyPart, HediffDefOf_CosmosInd.CosmosTech);
        }
    }
}
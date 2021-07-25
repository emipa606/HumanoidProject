using System.Text;
using RimWorld;
using Verse;

namespace HediffSpecial
{
    // Token: 0x0200001A RID: 26
    public class Hediff_RegrowinBodyPart : Hediff_AddedPart
    {
        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000052 RID: 82 RVA: 0x00003DEC File Offset: 0x00001FEC
        public override bool ShouldRemove => Severity >= def.maxSeverity;

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000054 RID: 84 RVA: 0x00003E0C File Offset: 0x0000200C
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

        // Token: 0x06000053 RID: 83 RVA: 0x00003E04 File Offset: 0x00002004

        // Token: 0x06000055 RID: 85 RVA: 0x00003EAC File Offset: 0x000020AC
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
}
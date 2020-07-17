using System;
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
		public override bool ShouldRemove
		{
			get
			{
				return this.Severity >= this.def.maxSeverity;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003E04 File Offset: 0x00002004
		public override void ExposeData()
		{
			base.ExposeData();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003E0C File Offset: 0x0000200C
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				stringBuilder.AppendLine(Translator.Translate("Efficiency") + ": " + this.def.addedPartProps.partEfficiency.ToStringPercent());
				stringBuilder.AppendLine(this.pawn.health.hediffSet.GetFirstHediffOfDef(this.def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff, false).def.GetModExtension<DefModExtension_BionicSpecial>().growthText + this.Severity.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003EAC File Offset: 0x000020AC
		public override void PostRemoved()
		{
			base.PostRemoved();
			bool flag = this.Severity >= 1f;
			if (flag && this.pawn.health.hediffSet.GetFirstHediffOfDef(this.def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff, false).def.TryGetModExtension<DefModExtension_BionicSpecial>().curedBodyPart != null)
			{
				this.pawn.ReplaceHediffFromBodypart(base.Part, HediffDefOf.MissingBodyPart, this.pawn.health.hediffSet.GetFirstHediffOfDef(this.def.GetModExtension<DefModExtension_BionicSpecial>().autoHealHediff, false).def.GetModExtension<DefModExtension_BionicSpecial>().curedBodyPart);
				return;
			}
			if (flag)
			{
				this.pawn.ReplaceHediffFromBodypart(base.Part, HediffDefOf.MissingBodyPart, HediffDefOf_CosmosInd.CosmosTech);
			}
		}
	}
}

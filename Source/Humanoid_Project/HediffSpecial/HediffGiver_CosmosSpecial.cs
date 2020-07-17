using System;
using Verse;

namespace HediffSpecial
{
	// Token: 0x0200001B RID: 27
	public class HediffGiver_CosmosSpecial : HediffGiver
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003F7F File Offset: 0x0000217F
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			base.TryApply(pawn, null);
		}
	}
}

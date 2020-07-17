using System;
using Verse;
using Verse.AI;

namespace EvaineQMentalWorker
{
	// Token: 0x02000032 RID: 50
	public class MentalStateWorker_StormMark : MentalStateWorker
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00005600 File Offset: 0x00003800
		public override bool StateCanOccur(Pawn pawn)
		{
			return pawn.Map != null && base.StateCanOccur(pawn) && pawn.Map.mapPawns.FreeColonistsSpawnedCount > 1;
		}
	}
}

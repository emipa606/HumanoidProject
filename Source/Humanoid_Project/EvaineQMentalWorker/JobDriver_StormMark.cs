using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace EvaineQMentalWorker
{
	// Token: 0x02000030 RID: 48
	public class JobDriver_StormMark : JobDriver
	{
		// Token: 0x06000090 RID: 144 RVA: 0x0000542E File Offset: 0x0000362E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005434 File Offset: 0x00003634
		private static Toil AbuseTarget(Pawn target)
		{
			Toil toil = new Toil();
			toil.AddFailCondition(() => target == null || target.Destroyed || target.Downed || !target.Spawned || target.Dead);
			toil.socialMode = RandomSocialMode.Off;
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (Rand.Value < 0.3f)
				{
					actor.interactions.TryInteractWith(target, InteractionDefOf.Chitchat);
					return;
				}
				actor.interactions.TryInteractWith(target, InteractionDefOf.DeepTalk);
			};
			return toil;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000549C File Offset: 0x0000369C
		private static Toil ReachTarget(Pawn target)
		{
			Toil toil = new Toil();
			toil.AddFailCondition(() => target == null || target.Destroyed || target.Downed || !target.Spawned || target.Dead);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.initAction = delegate()
			{
				toil.GetActor().pather.StartPath(target, PathEndMode.Touch);
			};
			return toil;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000550D File Offset: 0x0000370D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			Pawn target = base.TargetA.Thing as Pawn;
			yield return JobDriver_StormMark.ReachTarget(target);
			yield return JobDriver_StormMark.AbuseTarget(target);
			yield break;
		}
	}
}

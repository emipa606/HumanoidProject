using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace EvaineQMentalWorker
{
	// Token: 0x02000031 RID: 49
	public class JobGiver_StormMark : ThinkNode_JobGiver
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00005528 File Offset: 0x00003728
		protected override Job TryGiveJob(Pawn pawn)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			if (pawn.interactions.InteractedTooRecentlyToInteract() || lastInteractionTick > Find.TickManager.TicksGame - 500)
			{
				return null;
			}
			Pawn val = (Pawn)(object)(Pawn)GenClosest.ClosestThingReachable(((Thing)pawn).Position, ((Thing)pawn).Map, ThingRequest.ForGroup((ThingRequestGroup)11), (PathEndMode)1, TraverseParms.For(pawn, (Danger)3, (TraverseMode)0, false), 9999f, (Predicate<Thing>)validator, (IEnumerable<Thing>)null, 0, -1, false, (RegionType)6, false);
			if (val == null || Rand.Value > 0.5f)
			{
				return null;
			}
			lastInteractionTick = Find.TickManager.TicksGame;
			return (Job)(object)new Job(JobDefOfEvaineQMentalWorker.StormMark, (LocalTargetInfo)val);
			bool validator(Thing t)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Expected O, but got Unknown
				Pawn val2 = (Pawn)(object)(Pawn)t;
				if (val2 != pawn && !val2.Dead && !val2.Downed && RestUtility.Awake(val2) && InteractionUtility.CanReceiveInteraction(val2))
				{
					return val2.RaceProps.Humanlike;
				}
				return false;
			}
		}

		// Token: 0x04000048 RID: 72
		private int lastInteractionTick = -9999;
	}
}

using RimWorld;
using Verse;
using Verse.AI;

namespace EvaineQMentalWorker;

public class JobGiver_StormMark : ThinkNode_JobGiver
{
    private int lastInteractionTick = -9999;

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
        if (pawn.interactions.InteractedTooRecentlyToInteract() ||
            lastInteractionTick > Find.TickManager.TicksGame - 500)
        {
            return null;
        }

        var val = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map,
            ThingRequest.ForGroup((ThingRequestGroup)11), (PathEndMode)1, TraverseParms.For(pawn), 9999f,
            validator, null, 0, -1, false, (RegionType)6);
        if (val == null || Rand.Value > 0.5f)
        {
            return null;
        }

        lastInteractionTick = Find.TickManager.TicksGame;
        return new Job(JobDefOfEvaineQMentalWorker.StormMark, (LocalTargetInfo)val);

        bool validator(Thing t)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0007: Expected O, but got Unknown
            var val2 = (Pawn)t;
            if (val2 != pawn && !val2.Dead && !val2.Downed && val2.Awake() &&
                InteractionUtility.CanReceiveInteraction(val2))
            {
                return val2.RaceProps.Humanlike;
            }

            return false;
        }
    }
}
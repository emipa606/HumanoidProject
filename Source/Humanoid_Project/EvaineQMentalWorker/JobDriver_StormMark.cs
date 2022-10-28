using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace EvaineQMentalWorker;

public class JobDriver_StormMark : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    private static Toil AbuseTarget(Pawn target)
    {
        var toil = new Toil();
        toil.AddFailCondition(() =>
            target == null || target.Destroyed || target.Downed || !target.Spawned || target.Dead);
        toil.socialMode = RandomSocialMode.Off;
        toil.initAction = delegate
        {
            var actor = toil.GetActor();
            if (Rand.Value < 0.3f)
            {
                actor.interactions.TryInteractWith(target, InteractionDefOf.Chitchat);
                return;
            }

            actor.interactions.TryInteractWith(target, InteractionDefOf.DeepTalk);
        };
        return toil;
    }

    private static Toil ReachTarget(Pawn target)
    {
        var toil = new Toil();
        toil.AddFailCondition(() =>
            target == null || target.Destroyed || target.Downed || !target.Spawned || target.Dead);
        toil.socialMode = RandomSocialMode.Off;
        toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
        toil.initAction = delegate { toil.GetActor().pather.StartPath(target, PathEndMode.Touch); };
        return toil;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedOrNull(TargetIndex.A);
        this.FailOnDowned(TargetIndex.A);
        var target = TargetA.Thing as Pawn;
        yield return ReachTarget(target);
        yield return AbuseTarget(target);
    }
}
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace EvaineQBionics;

public class JobDriver_BlueVomit : JobDriver
{
    private int ticksLeft;

    public override void SetInitialPosture()
    {
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ticksLeft, "ticksLeft");
    }

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var toil = new Toil
        {
            initAction = delegate
            {
                ticksLeft = Rand.Range(300, 900);
                var num = 0;
                IntVec3 c;
                do
                {
                    c = pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
                    num++;
                    if (num > 12)
                    {
                        goto IL_6B;
                    }
                } while (!c.InBounds(pawn.Map) || !c.Standable(pawn.Map));

                goto IL_77;
                IL_6B:
                c = pawn.Position;
                IL_77:
                job.targetA = c;
                pawn.pather.StopDead();
            },
            tickAction = delegate
            {
                if (ticksLeft % 150 == 149)
                {
                    FilthMaker.TryMakeFilth(job.targetA.Cell, Map, ThingDefOf.Filth_BlueVomit,
                        pawn.LabelIndefinite());
                    if (pawn.needs.food.CurLevelPercentage > 0.1f)
                    {
                        pawn.needs.food.CurLevel -= pawn.needs.food.MaxLevel * 0.04f;
                    }
                }

                ticksLeft--;
                if (ticksLeft > 0)
                {
                    return;
                }

                ReadyForNextToil();
                TaleRecorder.RecordTale(TaleDefOf.Vomited, pawn);
            },
            defaultCompleteMode = ToilCompleteMode.Never
        };
        toil.WithEffect(EffecterDefOf.Blue_Vomit, TargetIndex.A);
        toil.PlaySustainerOrSound(() => SoundDefOf.Vomit);
        yield return toil;
    }
}
using System.Linq;
using RimWorld;
using Verse;

namespace HediffSpecial;

public class Hediff_EnchantedBionic : HediffWithComps
{
    public int ticksUntilNextGrow;

    public int ticksUntilNextHeal;

    public override void PostMake()
    {
        base.PostMake();
        SetNextHealTick();
        SetNextGrowTick();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ticksUntilNextHeal, "ticksUntilNextHeal");
    }

    public override void Tick()
    {
        base.Tick();
        if (Current.Game.tickManager.TicksGame >= ticksUntilNextHeal)
        {
            TrySealWounds();
            SetNextHealTick();
        }

        if (Current.Game.tickManager.TicksGame < ticksUntilNextGrow ||
            !def.TryGetModExtension<DefModExtension_BionicSpecial>().regrowParts)
        {
            return;
        }

        TryRegrowBodyparts();
        SetNextGrowTick();
    }

    public void TrySealWounds()
    {
        var enumerable = from hd in pawn.health.hediffSet.hediffs
            where hd.TendableNow()
            select hd;

        foreach (var hediff in enumerable)
        {
            if (hediff is not HediffWithComps hediffWithComps)
            {
                continue;
            }

            var hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
            hediffComp_TendDuration.tendQuality = 2f;
            hediffComp_TendDuration.tendTicksLeft = Find.TickManager.TicksGame;
            pawn.health.Notify_HediffChanged(hediff);
        }
    }

    public void TryRegrowBodyparts()
    {
        if (def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart != null)
        {
            using var enumerator = pawn.GetFirstMatchingBodyparts(pawn.RaceProps.body.corePart,
                HediffDefOf.MissingBodyPart, def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart,
                hediff => hediff is Hediff_AddedPart).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var part = enumerator.Current;
                var hediff3 = pawn.health.hediffSet.hediffs.First(hediff =>
                    hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart);

                pawn.health.RemoveHediff(hediff3);
                pawn.health.AddHediff(def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart,
                    part);
                pawn.health.hediffSet.DirtyCache();
            }

            return;
        }

        using var enumerator2 = pawn.GetFirstMatchingBodyparts(pawn.RaceProps.body.corePart,
            HediffDefOf.MissingBodyPart, HediffDefOf_CosmosInd.CosmosRegrowingTech,
            hediff => hediff is Hediff_AddedPart).GetEnumerator();
        while (enumerator2.MoveNext())
        {
            var part = enumerator2.Current;
            var hediff2 = pawn.health.hediffSet.hediffs.First(hediff =>
                hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart);

            pawn.health.RemoveHediff(hediff2);
            pawn.health.AddHediff(HediffDefOf_CosmosInd.CosmosRegrowingTech, part);
            pawn.health.hediffSet.DirtyCache();
        }
    }

    public void SetNextHealTick()
    {
        ticksUntilNextHeal = Current.Game.tickManager.TicksGame +
                             def.TryGetModExtension<DefModExtension_BionicSpecial>().healTicks;
    }

    public void SetNextGrowTick()
    {
        ticksUntilNextGrow = Current.Game.tickManager.TicksGame +
                             def.TryGetModExtension<DefModExtension_BionicSpecial>().growthTicks;
    }
}
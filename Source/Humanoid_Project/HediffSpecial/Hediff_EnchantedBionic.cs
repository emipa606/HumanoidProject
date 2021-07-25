using System.Linq;
using RimWorld;
using Verse;

namespace HediffSpecial
{
    // Token: 0x02000019 RID: 25
    public class Hediff_EnchantedBionic : HediffWithComps
    {
        // Token: 0x04000030 RID: 48
        public int ticksUntilNextGrow;

        // Token: 0x0400002F RID: 47
        public int ticksUntilNextHeal;

        // Token: 0x0600004A RID: 74 RVA: 0x000039E7 File Offset: 0x00001BE7
        public override void PostMake()
        {
            base.PostMake();
            SetNextHealTick();
            SetNextGrowTick();
        }

        // Token: 0x0600004B RID: 75 RVA: 0x000039FB File Offset: 0x00001BFB
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksUntilNextHeal, "ticksUntilNextHeal");
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00003A18 File Offset: 0x00001C18
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

        // Token: 0x0600004D RID: 77 RVA: 0x00003A84 File Offset: 0x00001C84
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

        // Token: 0x0600004E RID: 78 RVA: 0x00003B44 File Offset: 0x00001D44
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

        // Token: 0x0600004F RID: 79 RVA: 0x00003D94 File Offset: 0x00001F94
        public void SetNextHealTick()
        {
            ticksUntilNextHeal = Current.Game.tickManager.TicksGame +
                                 def.TryGetModExtension<DefModExtension_BionicSpecial>().healTicks;
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00003DBC File Offset: 0x00001FBC
        public void SetNextGrowTick()
        {
            ticksUntilNextGrow = Current.Game.tickManager.TicksGame +
                                 def.TryGetModExtension<DefModExtension_BionicSpecial>().growthTicks;
        }
    }
}
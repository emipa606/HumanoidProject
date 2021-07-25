using AbilityUser;
using Verse;

namespace HumanAbilities
{
    // Token: 0x02000009 RID: 9
    public class CompPsionicUserExample : CompAbilityUser
    {
        // Token: 0x0400000F RID: 15
        private bool firstTick;

        // Token: 0x0400000E RID: 14
        private bool gaveAbilities;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000019 RID: 25 RVA: 0x000029A0 File Offset: 0x00000BA0
        private bool IsHumanRace
        {
            get
            {
                var pawn = Pawn;
                bool hasHediff;
                if (pawn == null)
                {
                    hasHediff = false;
                }
                else
                {
                    var health = pawn.health;
                    hasHediff = health?.hediffSet != null;
                }

                return hasHediff && Pawn.health.hediffSet.HasHediff(HediffDef.Named("RedLightningImpact"));
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000029F4 File Offset: 0x00000BF4
        public override bool TryTransformPawn()
        {
            return IsHumanRace;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x000029FC File Offset: 0x00000BFC
        public override void CompTick()
        {
            var pawn = Pawn;
            if (pawn == null || !pawn.Spawned)
            {
                return;
            }

            if (Find.TickManager.TicksGame <= 200 || !IsHumanRace)
            {
                return;
            }

            if (!firstTick)
            {
                PostInitializeTick();
            }

            base.CompTick();
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002A50 File Offset: 0x00000C50
        private void PostInitializeTick()
        {
            var pawn = Pawn;
            if (pawn == null || !pawn.Spawned)
            {
                return;
            }

            if (Pawn.story != null &&
                Pawn.story.DisabledWorkTagsBackstoryAndTraits.OverlapsWithOnAnyWorkType(WorkTags.Violent))
            {
                return;
            }

            firstTick = true;
            Initialize();
            if (gaveAbilities)
            {
                return;
            }

            gaveAbilities = true;
            AddPawnAbility(HumanAbilitiesDefOf.HumanPower);
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00002AC3 File Offset: 0x00000CC3
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref gaveAbilities, "gaveAbilities");
        }
    }
}
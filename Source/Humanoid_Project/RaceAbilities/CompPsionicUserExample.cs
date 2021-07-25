using AbilityUser;
using Verse;

namespace RaceAbilities
{
    // Token: 0x02000042 RID: 66
    public class CompPsionicUserExample : CompAbilityUser
    {
        // Token: 0x04000078 RID: 120
        private bool firstTick;

        // Token: 0x04000077 RID: 119
        private bool gaveAbilities;

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x060000DF RID: 223 RVA: 0x00006F8C File Offset: 0x0000518C
        private bool IsAvianRace
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

                return hasHediff && Pawn.health.hediffSet.HasHediff(HediffDef.Named("AvianHeDiff"));
            }
        }

        // Token: 0x060000E0 RID: 224 RVA: 0x00006FE0 File Offset: 0x000051E0
        public override bool TryTransformPawn()
        {
            return IsAvianRace;
        }

        // Token: 0x060000E1 RID: 225 RVA: 0x00006FE8 File Offset: 0x000051E8
        public override void CompTick()
        {
            var pawn = Pawn;
            if (pawn == null || !pawn.Spawned)
            {
                return;
            }

            if (Find.TickManager.TicksGame <= 200 || !IsAvianRace)
            {
                return;
            }

            if (!firstTick)
            {
                PostInitializeTick();
            }

            base.CompTick();
        }

        // Token: 0x060000E2 RID: 226 RVA: 0x0000703C File Offset: 0x0000523C
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
            AddPawnAbility(RaceAbilitiesDefOf.AvianPower);
        }

        // Token: 0x060000E3 RID: 227 RVA: 0x000070AF File Offset: 0x000052AF
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref gaveAbilities, "gaveAbilities");
        }
    }
}
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HumanAbilities
{
    // Token: 0x02000002 RID: 2
    public class DamageWorker_RedFire : DamageWorker_AddInjury
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            var pawn = victim as Pawn;
            if (pawn != null && pawn.Faction == Faction.OfPlayer)
            {
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }

            var map = victim.Map;
            var damageResult = base.Apply(dinfo, victim);
            if (!damageResult.deflected && !dinfo.InstantPermanentInjury)
            {
                victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
            }

            if (!victim.Destroyed || map == null || pawn != null)
            {
                return damageResult;
            }

            foreach (var intVec in victim.OccupiedRect())
            {
                FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash);
            }

            if (victim is Plant plant && victim.def.plant.IsTree && plant.LifeStage != PlantLifeStage.Sowing &&
                victim.def != ThingDefOf.BurnedTree)
            {
                ((DeadPlant) GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map)).Growth = plant.Growth;
            }

            return damageResult;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002174 File Offset: 0x00000374
        public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings,
            List<Thing> ignoredThings, bool canThrowMotes)
        {
            base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
            if (def == DamageDefOf.RedFire && Rand.Chance(RedFireUtility.ChanceToStartFireIn(c, explosion.Map)))
            {
                RedFireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
            }
        }
    }
}
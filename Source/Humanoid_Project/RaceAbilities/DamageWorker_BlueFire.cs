using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaceAbilities;

public class DamageWorker_BlueFire : DamageWorker_AddInjury
{
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
            ((DeadPlant)GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map)).Growth = plant.Growth;
        }

        return damageResult;
    }

    public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings,
        List<Thing> ignoredThings, bool canThrowMotes)
    {
        base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
        if (def == DamageDefOf.BlueFire && Rand.Chance(BlueFireUtility.ChanceToStartFireIn(c, explosion.Map)))
        {
            BlueFireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
        }
    }
}
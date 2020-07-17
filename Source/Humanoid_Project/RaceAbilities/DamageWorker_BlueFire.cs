using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaceAbilities
{
	// Token: 0x02000041 RID: 65
	public class DamageWorker_BlueFire : DamageWorker_AddInjury
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00006E0C File Offset: 0x0000500C
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			Pawn pawn = victim as Pawn;
			if (pawn != null && pawn.Faction == Faction.OfPlayer)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			Map map = victim.Map;
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			if (!damageResult.deflected && !dinfo.InstantPermanentInjury)
			{
				victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
			}
			if (victim.Destroyed && map != null && pawn == null)
			{
				foreach (IntVec3 intVec in victim.OccupiedRect())
				{
					FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash, 1);
				}
				Plant plant = victim as Plant;
				if (plant != null && victim.def.plant.IsTree && plant.LifeStage != PlantLifeStage.Sowing && victim.def != ThingDefOf.BurnedTree)
				{
					((DeadPlant)GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map, WipeMode.Vanish)).Growth = plant.Growth;
				}
			}
			return damageResult;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006F30 File Offset: 0x00005130
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
			if (this.def == DamageDefOf.BlueFire && Rand.Chance(BlueFireUtility.ChanceToStartFireIn(c, explosion.Map)))
			{
				BlueFireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
			}
		}
	}
}

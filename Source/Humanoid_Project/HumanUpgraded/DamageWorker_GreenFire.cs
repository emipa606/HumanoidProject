using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HumanUpgraded
{
	// Token: 0x0200000A RID: 10
	public class DamageWorker_GreenFire : DamageWorker_AddInjury
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002AE8 File Offset: 0x00000CE8
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
                if (victim is Plant plant && victim.def.plant.IsTree && plant.LifeStage != PlantLifeStage.Sowing && victim.def != ThingDefOf.BurnedTree)
                {
                    ((DeadPlant)GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map, WipeMode.Vanish)).Growth = plant.Growth;
                }
            }
			return damageResult;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C0C File Offset: 0x00000E0C
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
			if (this.def == DamageDefOf.GreenFire && Rand.Chance(GreenFireUtility.ChanceToStartFireIn(c, explosion.Map)))
			{
				GreenFireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
			}
		}
	}
}

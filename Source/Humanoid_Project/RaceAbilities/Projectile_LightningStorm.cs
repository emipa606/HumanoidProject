using System;
using AbilityUser;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceAbilities
{
	// Token: 0x02000046 RID: 70
	public class Projectile_LightningStorm : Projectile_Ability
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000072F4 File Offset: 0x000054F4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.age < 600)
			{
				return;
			}
			base.Destroy(mode);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000730C File Offset: 0x0000550C
		public void ThrowBolt(IntVec3 strikeZone, Pawn victim)
		{
			if (!this.thrown)
			{
				this.thrown = true;
				this.strikeLoc = strikeZone;
				SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(null);
				this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
				GenExplosion.DoExplosion(this.strikeLoc, victim.Map, 3.5f, DamageDefOf.BlueFire, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				Vector3 loc = this.strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					MoteMaker.ThrowAvianSmoke(loc, victim.Map, 1.5f);
					MoteMaker.ThrowAvianMicroSparks(loc, victim.Map);
					MoteMaker.ThrowAvianLightningGlow(loc, victim.Map, 15.5f);
				}
				SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, victim.Map, false), MaintenanceType.None);
				SoundDefOf.Thunder_OnMap.PlayOneShot(info);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000073F0 File Offset: 0x000055F0
		public override void PostImpactEffects(Thing hitThing)
		{
			if (hitThing != null)
			{
				Pawn pawn = hitThing as Pawn;
				if (pawn != null)
				{
					this.duration = Rand.Range(30, 60);
					this.ThrowBolt(pawn.Position, pawn);
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007426 File Offset: 0x00005626
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000743C File Offset: 0x0000563C
		protected float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1.3f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000746C File Offset: 0x0000566C
		public override void Draw()
		{
			if (this.boltMesh != null)
			{
				Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller), Quaternion.identity, FadedMaterialPool.FadedVersionOf((Material)AccessTools.Field(typeof(WeatherEvent_LightningStrike), "LightningMat").GetValue(null), this.LightningBrightness), 0);
			}
		}

		// Token: 0x04000080 RID: 128
		private Mesh boltMesh;

		// Token: 0x04000081 RID: 129
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04000082 RID: 130
		private int age;

		// Token: 0x04000083 RID: 131
		private int duration;

		// Token: 0x04000084 RID: 132
		private bool thrown;
	}
}

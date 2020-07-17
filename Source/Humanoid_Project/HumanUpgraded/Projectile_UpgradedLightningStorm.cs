using System;
using AbilityUser;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace HumanUpgraded
{
	// Token: 0x0200000F RID: 15
	public class Projectile_UpgradedLightningStorm : Projectile_Ability
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003248 File Offset: 0x00001448
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.age < 600)
			{
				return;
			}
			base.Destroy(mode);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003260 File Offset: 0x00001460
		public void ThrowBolt(IntVec3 strikeZone, Pawn victim)
		{
			if (!this.thrown)
			{
				this.thrown = true;
				this.strikeLoc = strikeZone;
				SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(null);
				this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
				GenExplosion.DoExplosion(this.strikeLoc, victim.Map, 3f, DamageDefOf.GreenFire, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				Vector3 loc = this.strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					MoteMaker.ThrowUpgradedSmoke(loc, victim.Map, 1.5f);
					MoteMaker.ThrowUpgradedMicroSparks(loc, victim.Map);
					MoteMaker.ThrowUpgradedLightningGlow(loc, victim.Map, 15.5f);
				}
				SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, victim.Map, false), MaintenanceType.None);
				SoundDefOf.Thunder_OnMap.PlayOneShot(info);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003344 File Offset: 0x00001544
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

		// Token: 0x06000033 RID: 51 RVA: 0x0000337A File Offset: 0x0000157A
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00003390 File Offset: 0x00001590
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

		// Token: 0x06000035 RID: 53 RVA: 0x000033C0 File Offset: 0x000015C0
		public override void Draw()
		{
			if (this.boltMesh != null)
			{
				Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller), Quaternion.identity, FadedMaterialPool.FadedVersionOf((Material)AccessTools.Field(typeof(WeatherEvent_LightningStrike), "LightningMat").GetValue(null), this.LightningBrightness), 0);
			}
		}

		// Token: 0x04000017 RID: 23
		private Mesh boltMesh;

		// Token: 0x04000018 RID: 24
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04000019 RID: 25
		private int age;

		// Token: 0x0400001A RID: 26
		private int duration;

		// Token: 0x0400001B RID: 27
		private bool thrown;
	}
}

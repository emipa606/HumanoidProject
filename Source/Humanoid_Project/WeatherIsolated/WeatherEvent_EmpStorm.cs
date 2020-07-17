using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WeatherIsolated
{
	// Token: 0x02000014 RID: 20
	[StaticConstructorOnStartup]
	public class WeatherEvent_EmpStorm : WeatherEvent_LightningFlash
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000037A0 File Offset: 0x000019A0
		public WeatherEvent_EmpStorm(Map map) : base(map)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000037B4 File Offset: 0x000019B4
		public WeatherEvent_EmpStorm(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000037D0 File Offset: 0x000019D0
		public override void FireEvent()
		{
			base.FireEvent();
			if (!this.strikeLoc.IsValid)
			{
				this.strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(this.map) && !this.map.roofGrid.Roofed(sq), this.map, 1000);
			}
			this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			if (!this.strikeLoc.Fogged(this.map))
			{
				GenExplosion.DoExplosion(this.strikeLoc, this.map, 8f, DamageDefOf.EMP, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				Vector3 loc = this.strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					MoteMaker.ThrowEMPSmoke(loc, this.map, 2f);
					MoteMaker.ThrowEMPMicroSparks(loc, this.map);
					MoteMaker.ThrowEMPLightningGlow(loc, this.map, 4.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, this.map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000038D8 File Offset: 0x00001AD8
		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_EmpStorm.LightningMat, base.LightningBrightness), 0);
		}

		// Token: 0x04000022 RID: 34
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04000023 RID: 35
		private Mesh boltMesh;

		// Token: 0x04000024 RID: 36
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}

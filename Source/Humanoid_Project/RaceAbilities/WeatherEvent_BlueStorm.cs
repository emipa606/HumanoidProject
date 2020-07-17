using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceAbilities
{
	// Token: 0x0200003E RID: 62
	[StaticConstructorOnStartup]
	public class WeatherEvent_BlueStorm : WeatherEvent_LightningFlash
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000068AC File Offset: 0x00004AAC
		public WeatherEvent_BlueStorm(Map map) : base(map)
		{
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000068C0 File Offset: 0x00004AC0
		public WeatherEvent_BlueStorm(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000068DC File Offset: 0x00004ADC
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
				GenExplosion.DoExplosion(this.strikeLoc, this.map, 3.9f, DamageDefOf.BlueFire, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				Vector3 loc = this.strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					MoteMaker.ThrowAvianSmoke(loc, this.map, 1.5f);
					MoteMaker.ThrowAvianMicroSparks(loc, this.map);
					MoteMaker.ThrowAvianLightningGlow(loc, this.map, 4.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, this.map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000069E4 File Offset: 0x00004BE4
		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_BlueStorm.LightningMat, base.LightningBrightness), 0);
		}

		// Token: 0x04000073 RID: 115
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04000074 RID: 116
		private Mesh boltMesh;

		// Token: 0x04000075 RID: 117
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}

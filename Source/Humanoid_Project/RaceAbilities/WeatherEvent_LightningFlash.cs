using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceAbilities
{
	// Token: 0x0200003D RID: 61
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00006778 File Offset: 0x00004978
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000067C4 File Offset: 0x000049C4
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000067D4 File Offset: 0x000049D4
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000067EF File Offset: 0x000049EF
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000067FC File Offset: 0x000049FC
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00006804 File Offset: 0x00004A04
		protected float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006832 File Offset: 0x00004A32
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006844 File Offset: 0x00004A44
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x0400006B RID: 107
		private int duration;

		// Token: 0x0400006C RID: 108
		private Vector2 shadowVector;

		// Token: 0x0400006D RID: 109
		private int age;

		// Token: 0x0400006E RID: 110
		private const int FlashFadeInTicks = 5;

		// Token: 0x0400006F RID: 111
		private const int MinFlashDuration = 15;

		// Token: 0x04000070 RID: 112
		private const int MaxFlashDuration = 30;

		// Token: 0x04000071 RID: 113
		private const float FlashShadowDistance = 25f;

		// Token: 0x04000072 RID: 114
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.784313738f, 0.8235294f, 0.847058833f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}

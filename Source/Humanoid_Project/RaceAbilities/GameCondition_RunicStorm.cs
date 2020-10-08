using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RaceAbilities
{
	// Token: 0x0200003A RID: 58
	public class GameCondition_RunicStorm : GameCondition
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x000063BC File Offset: 0x000045BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default, false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006410 File Offset: 0x00004610
		public override void Init()
		{
			base.Init();
			this.areaRadius = GameCondition_RunicStorm.AreaRadiusRange.RandomInRange;
			this.FindGoodCenterLocation();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000643C File Offset: 0x0000463C
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_BlueStorm(base.SingleMap, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_RunicStorm.TicksBetweenStrikes.RandomInRange;
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000064FC File Offset: 0x000046FC
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000651C File Offset: 0x0000471C
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for Runic Storm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000065B2 File Offset: 0x000047B2
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000065E0 File Offset: 0x000047E0
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.14159274f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006660 File Offset: 0x00004860
		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			int num;
			for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x = num + 1)
			{
				for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z = num + 1)
				{
					if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
					{
						yield return new IntVec3(x, 0, z);
					}
					num = z;
				}
				num = x;
			}
			yield break;
		}

		// Token: 0x04000064 RID: 100
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x04000065 RID: 101
		private static readonly IntRange TicksBetweenStrikes = new IntRange(420, 900);

		// Token: 0x04000066 RID: 102
		private const int RainDisableTicksAfterConditionEnds = 1000;

		// Token: 0x04000067 RID: 103
		public IntVec2 centerLocation;

		// Token: 0x04000068 RID: 104
		private int areaRadius;

		// Token: 0x04000069 RID: 105
		private int nextLightningTicks;
	}
}

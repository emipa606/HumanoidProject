using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaceAbilities
{
    // Token: 0x0200003A RID: 58
    public class GameCondition_RunicStorm : GameCondition
    {
        // Token: 0x04000066 RID: 102
        private const int RainDisableTicksAfterConditionEnds = 1000;

        // Token: 0x04000064 RID: 100
        private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

        // Token: 0x04000065 RID: 101
        private static readonly IntRange TicksBetweenStrikes = new IntRange(420, 900);

        // Token: 0x04000068 RID: 104
        private int areaRadius;

        // Token: 0x04000067 RID: 103
        public IntVec2 centerLocation;

        // Token: 0x04000069 RID: 105
        private int nextLightningTicks;

        // Token: 0x060000B4 RID: 180 RVA: 0x000063BC File Offset: 0x000045BC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref centerLocation, "centerLocation");
            Scribe_Values.Look(ref areaRadius, "areaRadius");
            Scribe_Values.Look(ref nextLightningTicks, "nextLightningTicks");
        }

        // Token: 0x060000B5 RID: 181 RVA: 0x00006410 File Offset: 0x00004610
        public override void Init()
        {
            base.Init();
            areaRadius = AreaRadiusRange.RandomInRange;
            FindGoodCenterLocation();
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x0000643C File Offset: 0x0000463C
        public override void GameConditionTick()
        {
            if (Find.TickManager.TicksGame <= nextLightningTicks)
            {
                return;
            }

            var vector = Rand.UnitVector2 * Rand.Range(0f, areaRadius);
            var intVec = new IntVec3((int) Math.Round(vector.x) + centerLocation.x, 0,
                (int) Math.Round(vector.y) + centerLocation.z);
            if (!IsGoodLocationForStrike(intVec))
            {
                return;
            }

            SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_BlueStorm(SingleMap, intVec));
            nextLightningTicks = Find.TickManager.TicksGame + TicksBetweenStrikes.RandomInRange;
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x000064FC File Offset: 0x000046FC
        public override void End()
        {
            SingleMap.weatherDecider.DisableRainFor(30000);
            base.End();
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x0000651C File Offset: 0x0000471C
        private void FindGoodCenterLocation()
        {
            if (SingleMap.Size.x <= 16 || SingleMap.Size.z <= 16)
            {
                throw new Exception("Map too small for Runic Storm.");
            }

            for (var i = 0; i < 10; i++)
            {
                centerLocation = new IntVec2(Rand.Range(8, SingleMap.Size.x - 8), Rand.Range(8, SingleMap.Size.z - 8));
                if (IsGoodCenterLocation(centerLocation))
                {
                    break;
                }
            }
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x000065B2 File Offset: 0x000047B2
        private bool IsGoodLocationForStrike(IntVec3 loc)
        {
            return loc.InBounds(SingleMap) && !loc.Roofed(SingleMap) && loc.Standable(SingleMap);
        }

        // Token: 0x060000BA RID: 186 RVA: 0x000065E0 File Offset: 0x000047E0
        private bool IsGoodCenterLocation(IntVec2 loc)
        {
            var num = 0;
            var num2 = (int) (3.14159274f * areaRadius * areaRadius / 2f);
            foreach (var loc2 in GetPotentiallyAffectedCells(loc))
            {
                if (IsGoodLocationForStrike(loc2))
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
            for (var x = center.x - areaRadius; x <= center.x + areaRadius; x = num + 1)
            {
                for (var z = center.z - areaRadius; z <= center.z + areaRadius; z = num + 1)
                {
                    if (((center.x - x) * (center.x - x)) + ((center.z - z) * (center.z - z)) <=
                        areaRadius * areaRadius)
                    {
                        yield return new IntVec3(x, 0, z);
                    }

                    num = z;
                }

                num = x;
            }
        }
    }
}
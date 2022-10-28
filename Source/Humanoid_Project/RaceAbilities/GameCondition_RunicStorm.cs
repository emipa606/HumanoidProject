using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaceAbilities;

public class GameCondition_RunicStorm : GameCondition
{
    private const int RainDisableTicksAfterConditionEnds = 1000;

    private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

    private static readonly IntRange TicksBetweenStrikes = new IntRange(420, 900);

    private int areaRadius;

    public IntVec2 centerLocation;

    private int nextLightningTicks;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref centerLocation, "centerLocation");
        Scribe_Values.Look(ref areaRadius, "areaRadius");
        Scribe_Values.Look(ref nextLightningTicks, "nextLightningTicks");
    }

    public override void Init()
    {
        base.Init();
        areaRadius = AreaRadiusRange.RandomInRange;
        FindGoodCenterLocation();
    }

    public override void GameConditionTick()
    {
        if (Find.TickManager.TicksGame <= nextLightningTicks)
        {
            return;
        }

        var vector = Rand.UnitVector2 * Rand.Range(0f, areaRadius);
        var intVec = new IntVec3((int)Math.Round(vector.x) + centerLocation.x, 0,
            (int)Math.Round(vector.y) + centerLocation.z);
        if (!IsGoodLocationForStrike(intVec))
        {
            return;
        }

        SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_BlueStorm(SingleMap, intVec));
        nextLightningTicks = Find.TickManager.TicksGame + TicksBetweenStrikes.RandomInRange;
    }

    public override void End()
    {
        SingleMap.weatherDecider.DisableRainFor(30000);
        base.End();
    }

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

    private bool IsGoodLocationForStrike(IntVec3 loc)
    {
        return loc.InBounds(SingleMap) && !loc.Roofed(SingleMap) && loc.Standable(SingleMap);
    }

    private bool IsGoodCenterLocation(IntVec2 loc)
    {
        var num = 0;
        var num2 = (int)(3.14159274f * areaRadius * areaRadius / 2f);
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
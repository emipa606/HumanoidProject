﻿using UnityEngine;
using Verse;

namespace HumanUpgraded
{
    // Token: 0x0200000E RID: 14
    public static class MoteMaker
    {
        // Token: 0x0600002D RID: 45 RVA: 0x00003028 File Offset: 0x00001228
        public static void ThrowUpgradedMicroSparks(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.Sparks_GreenLightning);
            moteThrown.Scale = Rand.Range(5f, 8f);
            moteThrown.rotationRate = Rand.Range(-12f, 12f);
            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000030FC File Offset: 0x000012FC
        public static void ThrowUpgradedLightningGlow(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.AbilityGreenGlow);
            moteThrown.Scale = Rand.Range(6f, 8f) * size;
            moteThrown.rotationRate = Rand.Range(-3f, 3f);
            moteThrown.exactPosition = loc + (size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f));
            moteThrown.SetVelocity(Rand.Range(0, 360), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000031B4 File Offset: 0x000013B4
        public static void ThrowUpgradedSmoke(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.Mote_GreenSmoke);
            moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
            moteThrown.rotationRate = Rand.Range(-30f, 30f);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocity(Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }
    }
}
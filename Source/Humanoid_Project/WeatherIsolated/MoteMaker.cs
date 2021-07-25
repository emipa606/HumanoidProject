using UnityEngine;
using Verse;

namespace WeatherIsolated
{
    // Token: 0x02000013 RID: 19
    public static class MoteMaker
    {
        // Token: 0x0600003D RID: 61 RVA: 0x00003580 File Offset: 0x00001780
        public static void ThrowEMPMicroSparks(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.EMP_Sparks);
            moteThrown.Scale = Rand.Range(5f, 8f);
            moteThrown.rotationRate = Rand.Range(-12f, 12f);
            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00003654 File Offset: 0x00001854
        public static void ThrowEMPLightningGlow(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.EMPGlow);
            moteThrown.Scale = Rand.Range(6f, 8f) * size;
            moteThrown.rotationRate = Rand.Range(-3f, 3f);
            moteThrown.exactPosition = loc + (size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f));
            moteThrown.SetVelocity(Rand.Range(0, 360), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x0600003F RID: 63 RVA: 0x0000370C File Offset: 0x0000190C
        public static void ThrowEMPSmoke(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.Mote_EMPSmoke);
            moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
            moteThrown.rotationRate = Rand.Range(-30f, 30f);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocity(Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }
    }
}
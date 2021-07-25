using UnityEngine;
using Verse;

namespace RaceAbilities
{
    // Token: 0x02000045 RID: 69
    public static class MoteMaker
    {
        // Token: 0x060000E5 RID: 229 RVA: 0x000070D4 File Offset: 0x000052D4
        public static void ThrowAvianMicroSparks(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.Sparks_BlueLightning);
            moteThrown.Scale = Rand.Range(5f, 9f);
            moteThrown.rotationRate = Rand.Range(-12f, 12f);
            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x000071A8 File Offset: 0x000053A8
        public static void ThrowAvianLightningGlow(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.AbilityBlueGlow);
            moteThrown.Scale = Rand.Range(6f, 8f) * size;
            moteThrown.rotationRate = Rand.Range(-3f, 3f);
            moteThrown.exactPosition = loc + (size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f));
            moteThrown.SetVelocity(Rand.Range(0, 360), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }

        // Token: 0x060000E7 RID: 231 RVA: 0x00007260 File Offset: 0x00005460
        public static void ThrowAvianSmoke(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDefOf.Mote_BlueSmoke);
            moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
            moteThrown.rotationRate = Rand.Range(-30f, 30f);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocity(Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }
    }
}
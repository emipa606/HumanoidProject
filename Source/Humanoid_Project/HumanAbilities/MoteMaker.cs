using UnityEngine;
using Verse;

namespace HumanAbilities;

public static class MoteMaker
{
    public static void ThrowHumanMicroSparks(Vector3 loc, Map map)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
        {
            return;
        }

        var moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Sparks_RedLightning);
        moteThrown.Scale = Rand.Range(2f, 4f);
        moteThrown.rotationRate = Rand.Range(-12f, 12f);
        moteThrown.exactPosition = loc;
        moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
        moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
        moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
    }

    public static void ThrowHumanLightningGlow(Vector3 loc, Map map, float size)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
        {
            return;
        }

        var moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.AbilityRedGlow);
        moteThrown.Scale = Rand.Range(4f, 6f) * size;
        moteThrown.rotationRate = Rand.Range(-3f, 3f);
        moteThrown.exactPosition = loc + (size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f));
        moteThrown.SetVelocity(Rand.Range(0, 360), 1.2f);
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
    }

    public static void ThrowHumanSmoke(Vector3 loc, Map map, float size)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
        {
            return;
        }

        var moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_RedSmoke);
        moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
        moteThrown.rotationRate = Rand.Range(-30f, 30f);
        moteThrown.exactPosition = loc;
        moteThrown.SetVelocity(Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
    }
}
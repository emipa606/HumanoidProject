using RimWorld;
using UnityEngine;
using Verse;

namespace HumanUpgraded;

public static class GreenFireUtility
{
    public static bool CanEverAttachFire(this Thing t)
    {
        return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn &&
               t.TryGetComp<CompAttachBase>() != null;
    }

    public static float ChanceToStartFireIn(IntVec3 c, Map map)
    {
        var thingList = c.GetThingList(map);
        var num = !FireUtility.TerrainFlammableNow(c, map)
            ? 0f
            : c.GetTerrain(map).GetStatValueAbstract(StatDefOf.Flammability);
        foreach (var thing1 in thingList)
        {
            if (thing1 is Fire)
            {
                return 0f;
            }

            if (thing1.def.category != ThingCategory.Pawn && thing1.FlammableNow)
            {
                num = Mathf.Max(num, thing1.GetStatValue(StatDefOf.Flammability));
            }
        }

        if (!(num > 0f))
        {
            return num;
        }

        var edifice = c.GetEdifice(map);
        if (edifice != null && edifice.def.passability == Traversability.Impassable &&
            edifice.OccupiedRect().ContractedBy(1).Contains(c))
        {
            return 0f;
        }

        var thingList2 = c.GetThingList(map);
        foreach (var thing in thingList2)
        {
            if (thing.def.category == ThingCategory.Filth && !thing.def.filth.allowsFire)
            {
                return 0f;
            }
        }

        return num;
    }

    public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
    {
        if (ChanceToStartFireIn(c, map) <= 0f)
        {
            return false;
        }

        var fire = (Fire)ThingMaker.MakeThing(ThingDefOf.GreenFire);
        fire.fireSize = fireSize;
        GenSpawn.Spawn(fire, c, map, Rot4.North);
        return true;
    }

    public static void TryAttachFire(this Thing t, float fireSize)
    {
        if (t.Map == null)
        {
            return;
        }

        if (!t.Spawned)
        {
            return;
        }

        if (!t.CanEverAttachFire())
        {
            return;
        }

        if (t.HasAttachment(ThingDefOf.GreenFire))
        {
            return;
        }

        var fire = (Fire)ThingMaker.MakeThing(ThingDefOf.GreenFire);
        fire.fireSize = fireSize;
        fire.AttachTo(t);
        GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North);
        if (t is not Pawn pawn)
        {
            return;
        }

        pawn.jobs.StopAll();
        pawn.records.Increment(RecordDefOf.TimesOnFire);
    }

    public static bool IsBurning(this TargetInfo t)
    {
        return t.HasThing ? t.Thing.IsBurning() : t.Cell.ContainsStaticFire(t.Map);
    }

    public static bool IsBurning(this Thing t)
    {
        if (t.Destroyed || !t.Spawned)
        {
            return false;
        }

        if (!(t.def.size == IntVec2.One))
        {
            var iterator = t.OccupiedRect().GetIterator();
            while (!iterator.Done())
            {
                if (iterator.Current.ContainsStaticFire(t.Map))
                {
                    return true;
                }

                iterator.MoveNext();
            }

            return false;
        }

        if (t is Pawn)
        {
            return t.HasAttachment(ThingDefOf.GreenFire);
        }

        return t.Position.ContainsStaticFire(t.Map);
    }

    public static bool ContainsStaticFire(this IntVec3 c, Map map)
    {
        var list = map.thingGrid.ThingsListAt(c);
        foreach (var thing in list)
        {
            if (thing is Fire { parent: null })
            {
                return true;
            }
        }

        return false;
    }

    public static bool ContainsTrap(this IntVec3 c, Map map)
    {
        var edifice = c.GetEdifice(map);
        return edifice is Building_Trap;
    }

    public static bool Flammable(this TerrainDef terrain)
    {
        return terrain.GetStatValueAbstract(StatDefOf.Flammability) > 0.01f;
    }

    public static bool TerrainFlammableNow(this IntVec3 c, Map map)
    {
        if (!c.GetTerrain(map).Flammable())
        {
            return false;
        }

        var thingList = c.GetThingList(map);
        foreach (var thing in thingList)
        {
            if (thing.FireBulwark)
            {
                return false;
            }
        }

        return true;
    }
}
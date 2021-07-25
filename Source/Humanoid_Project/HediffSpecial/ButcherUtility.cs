using System;
using Verse;

namespace HediffSpecial
{
    // Token: 0x0200001D RID: 29
    public static class ButcherUtility
    {
        // Token: 0x0600005F RID: 95 RVA: 0x00004174 File Offset: 0x00002374
        public static void SpawnDrops(Pawn pawn, IntVec3 position, Map map)
        {
            var coverageOfNotMissingNaturalParts =
                pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
            foreach (var thingDefCountClass in pawn.def.butcherProducts)
            {
                var num = (int) Math.Ceiling(thingDefCountClass.count * coverageOfNotMissingNaturalParts);
                if (num <= 0)
                {
                    continue;
                }

                do
                {
                    var thing = ThingMaker.MakeThing(thingDefCountClass.thingDef);
                    thing.stackCount = Math.Min(num, thingDefCountClass.thingDef.stackLimit);
                    num -= thing.stackCount;
                    GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);
                } while (num > 0);
            }
        }
    }
}
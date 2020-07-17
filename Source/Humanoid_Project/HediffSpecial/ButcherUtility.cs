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
			float coverageOfNotMissingNaturalParts = pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
			foreach (ThingDefCountClass thingDefCountClass in pawn.def.butcherProducts)
			{
				int num = (int)Math.Ceiling((double)((float)thingDefCountClass.count * coverageOfNotMissingNaturalParts));
				if (num > 0)
				{
					do
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
						thing.stackCount = Math.Min(num, thingDefCountClass.thingDef.stackLimit);
						num -= thing.stackCount;
						GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near, null, null);
					}
					while (num > 0);
				}
			}
		}
	}
}

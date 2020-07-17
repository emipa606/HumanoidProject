using System;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace GenerationWorker
{
	// Token: 0x02000022 RID: 34
	public class GenStep_ItemGeneration : GenStep_Scatterer
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000047E1 File Offset: 0x000029E1
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000047E8 File Offset: 0x000029E8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 7, 7).GetIterator();
			while (!iterator.Done())
			{
				if (!iterator.Current.InBounds(map) || iterator.Current.GetEdifice(map) != null)
				{
					return false;
				}
				iterator.MoveNext();
			}
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004870 File Offset: 0x00002A70
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			ItemStashContentsComp component = map.Parent.GetComponent<ItemStashContentsComp>();
			if (component != null && component.contents.Any)
			{
				resolveParams.stockpileConcreteContents = component.contents;
			}
			else
			{
				resolveParams.thingSetMakerDef = (this.thingSetMakerDef ?? ThingSetMakerDefOf.RewardOptions);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("storage", resolveParams);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
		}

        // Token: 0x0400003A RID: 58
        public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400003B RID: 59
		private const int Size = 7;
	}
}

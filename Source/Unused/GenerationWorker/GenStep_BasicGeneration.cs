using System;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace GenerationWorker
{
	// Token: 0x02000021 RID: 33
	public class GenStep_BasicGeneration : GenStep
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004522 File Offset: 0x00002722
		public override int SeedPart
		{
			get
			{
				return 895502705;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000452C File Offset: 0x0000272C
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				cellRect = this.FindRandomRectToDefend(map);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = (from x in Find.FactionManager.AllFactions
				where !x.defeated && x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.techLevel >= TechLevel.Industrial
				select x).RandomElementWithFallback(Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Industrial));
			}
			else
			{
				faction = map.ParentFaction;
			}
			int randomInRange = this.widthRange.RandomInRange;
			CellRect rect = cellRect.ExpandedBy(7 + randomInRange).ClipInsideMap(map);
			int value;
			int value2;
			if (parms.siteCoreOrPart != null)
			{
				value = parms.siteCoreOrPart.parms.turretsCount;
				value2 = parms.siteCoreOrPart.parms.mortarsCount;
			}
			else
			{
				value = this.defaultTurretsCountRange.RandomInRange;
				value2 = this.defaultMortarsCountRange.RandomInRange;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(randomInRange);
			resolveParams.edgeDefenseTurretsCount = new int?(value);
			resolveParams.edgeDefenseMortarsCount = new int?(value2);
			resolveParams.edgeDefenseGuardsCount = new int?(this.guardsCountRange.RandomInRange);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("edgeDefense", resolveParams);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = rect;
			resolveParams2.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("hives", resolveParams2);
			BaseGen.Generate();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000046CC File Offset: 0x000028CC
		private CellRect FindRandomRectToDefend(Map map)
		{
			int rectRadius = Mathf.Max(Mathf.RoundToInt((float)Mathf.Min(map.Size.x, map.Size.z) * 0.07f), 1);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			IntVec3 center;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate(IntVec3 x)
			{
				if (!map.reachability.CanReachMapEdge(x, traverseParams))
				{
					return false;
				}
				CellRect cellRect = CellRect.CenteredOn(x, rectRadius);
				int num = 0;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					if (!iterator.Current.InBounds(map))
					{
						return false;
					}
					if (iterator.Current.Standable(map) || iterator.Current.GetPlant(map) != null)
					{
						num++;
					}
					iterator.MoveNext();
				}
				return (float)num / (float)cellRect.Area >= 0.6f;
			}, map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map), map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			return CellRect.CenteredOn(CellFinder.RandomCell(map), rectRadius).ClipInsideMap(map);
		}

		// Token: 0x04000034 RID: 52
		public IntRange defaultTurretsCountRange = new IntRange(4, 5);

		// Token: 0x04000035 RID: 53
		public IntRange defaultMortarsCountRange = new IntRange(0, 1);

		// Token: 0x04000036 RID: 54
		public IntRange widthRange = new IntRange(3, 4);

		// Token: 0x04000037 RID: 55
		public IntRange guardsCountRange = new IntRange(0, 0);

		// Token: 0x04000038 RID: 56
		private const int Padding = 7;

		// Token: 0x04000039 RID: 57
		public const int DefaultGuardsCount = 0;
	}
}

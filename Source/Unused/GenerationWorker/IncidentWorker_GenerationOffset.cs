using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GenerationWorker
{
	// Token: 0x02000023 RID: 35
	public class IncidentWorker_GenerationOffset : IncidentWorker
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00004920 File Offset: 0x00002B20
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Industrial) != null && this.TryFindTile(out num) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.OldOutpost, null, ref faction, true, null);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004964 File Offset: 0x00002B64
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction = parms.faction;
			if (faction == null)
			{
				faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Industrial);
			}
			if (faction == null)
			{
				return false;
			}
			int tile;
			if (!this.TryFindTile(out tile))
			{
				return false;
			}
			SitePartDef sitePart;
			Faction siteFaction;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.OldOutpost, Rand.Chance(1f) ? "SiteTreatSecured" : null, ref sitePart, ref siteFaction, null, true, null))
			{
				return false;
			}
			int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
			Site site = IncidentWorker_GenerationOffset.CreateSite(tile, sitePart, randomInRange, siteFaction);
			List<Thing> list = this.GenerateItems(siteFaction, site.desiredThreatPoints);
			site.GetComponent<ItemStashContentsComp>().contents.TryAddRangeOrTransfer(list, false, false);
			string letterText = this.GetLetterText(faction, list, randomInRange, site, site.parts.FirstOrDefault<SitePart>());
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, letterText, this.def.letterDef, site, faction, null);
			return true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004A4C File Offset: 0x00002C4C
		private bool TryFindTile(out int tile)
		{
			IntRange itemStashQuestSiteDistanceRange = SiteTuning.ItemStashQuestSiteDistanceRange;
			return TileFinder.TryFindNewSiteTile(out tile, itemStashQuestSiteDistanceRange.min, itemStashQuestSiteDistanceRange.max, false, true, -1);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004A74 File Offset: 0x00002C74
		protected virtual List<Thing> GenerateItems(Faction siteFaction, float siteThreatPoints)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(SiteTuning.ItemStashQuestMarketValueRange * SiteTuning.QuestRewardMarketValueThreatPointsFactor.Evaluate(siteThreatPoints));
			return ThingSetMakerDefOf.Gen_OldOutpost.root.Generate(parms);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004ABC File Offset: 0x00002CBC
		public static Site CreateSite(int tile, SitePartDef sitePart, int days, Faction siteFaction)
		{
			Site site = SiteMaker.MakeSite(SiteCoreDefOf.OldOutpost, sitePart, tile, siteFaction, true, null);
			site.sitePartsKnown = true;
			site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
			Find.WorldObjects.Add(site);
			return site;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004B08 File Offset: 0x00002D08
		private string GetLetterText(Faction alliedFaction, List<Thing> items, int days, Site site, SitePart sitePart)
		{
			string result = GrammarResolverSimpleStringExtensions.Formatted(this.def.letterText, alliedFaction.leader.LabelShort, alliedFaction.def.leaderTitle, alliedFaction.Name, GenLabel.ThingsLabel(items, "  - "), days.ToString(), SitePartUtility.GetDescriptionDialogue(site, sitePart), GenThing.GetMarketValue(items).ToStringMoney(null)).CapitalizeFirst();
			GenThing.TryAppendSingleRewardInfo(ref result, items);
			return result;
		}
	}
}

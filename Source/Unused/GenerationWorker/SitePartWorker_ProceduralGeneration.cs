using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace GenerationWorker
{
	// Token: 0x02000025 RID: 37
	public class SitePartWorker_ProceduralGeneration : SitePartWorker
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, ref preferredLetterDef, ref lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.HostileTo(Faction.OfPlayer)
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004C05 File Offset: 0x00002E05
		public virtual string GetPostProcessedDescriptionDialogue(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return string.Format(base.GetPostProcessedDescriptionDialogue(site, siteCoreOrPart), this.GetEnemiesCount(site, siteCoreOrPart.parms));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004C28 File Offset: 0x00002E28
		public override string GetPostProcessedThreatLabel(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return string.Concat(new object[]
			{
				base.GetPostProcessedThreatLabel(site, siteCoreOrPart),
				" (",
				this.GetEnemiesCount(site, siteCoreOrPart.parms),
				" ",
				Translator.Translate("Enemies"),
				")"
			});
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004C85 File Offset: 0x00002E85
		public virtual SiteCoreOrPartParams GenerateDefaultParams(Site site, float myThreatPoints)
		{
			SiteCoreOrPartParams siteCoreOrPartParams = base.GenerateDefaultParams(site, myThreatPoints);
			siteCoreOrPartParams.threatPoints = Mathf.Max(siteCoreOrPartParams.threatPoints, site.Faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
			return siteCoreOrPartParams;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004CB8 File Offset: 0x00002EB8
		private int GetEnemiesCount(Site site, SiteCoreOrPartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = site.Faction,
				groupKind = PawnGroupKindDefOf.Combat,
				points = parms.threatPoints,
				inhabitants = true,
				seed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}
	}
}

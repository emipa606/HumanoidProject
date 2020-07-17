using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RaceAbilities
{
	// Token: 0x02000039 RID: 57
	public class IncidentWorker_RaceRescuer : IncidentWorker
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00006174 File Offset: 0x00004374
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000061A4 File Offset: 0x000043A4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Gender? gender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				gender = new Gender?(this.def.pawnFixedGender);
			}
			PawnKindDef pawnKindDef = this.def.pawnKind;
			Faction ofPlayer = Faction.OfPlayer;
			List<PawnKindDef> list = (from def in DefDatabase<PawnKindDef>.AllDefs
			where def.race == ofPlayer.def.basicMemberKind.race && def.defName.Contains("BKRescuer")
			select def).ToList<PawnKindDef>();
			if (list.Count > 0)
			{
				pawnKindDef = list.RandomElement<PawnKindDef>();
			}
			else
			{
				list = (from def in DefDatabase<PawnKindDef>.AllDefs
				where def.defName.Contains("BKRescuer")
				select def).ToList<PawnKindDef>();
				pawnKindDef = list.RandomElement<PawnKindDef>();
			}
			pawnKindDef.defaultFactionType = ofPlayer.def;
			bool pawnMustBeCapableOfViolence = this.def.pawnMustBeCapableOfViolence;
			Gender? gender2 = gender;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, ofPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, pawnMustBeCapableOfViolence, 20f, false, true, true, false, false, false, false, false, 0, null, 1, null, null, null, null, null, null,null, gender2, null, null));
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			TaggedString text = GenText.AdjustedFor(GrammarResolverSimpleStringExtensions.Formatted(this.def.letterText, pawn.Named("PAWN")), pawn, "PAWN");
			TaggedString text2 = GenText.AdjustedFor(GrammarResolverSimpleStringExtensions.Formatted(this.def.letterLabel, pawn.Named("PAWN")), pawn, "PAWN");
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref text2, pawn);
			Find.LetterStack.ReceiveLetter(text2, text, LetterDefOf.PositiveEvent, pawn, null, null);
			return true;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000637C File Offset: 0x0000457C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x04000063 RID: 99
		private const float RelationWithColonistWeight = 20f;
	}
}

using System.Linq;
using RimWorld;
using Verse;

namespace RaceAbilities
{
    // Token: 0x02000039 RID: 57
    public class IncidentWorker_RaceRescuer : IncidentWorker
    {
        // Token: 0x04000063 RID: 99
        private const float RelationWithColonistWeight = 20f;

        // Token: 0x060000B0 RID: 176 RVA: 0x00006174 File Offset: 0x00004374
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }

            var map = (Map) parms.target;
            return TryFindEntryCell(map, out _);
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x000061A4 File Offset: 0x000043A4
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map) parms.target;
            if (!TryFindEntryCell(map, out var loc))
            {
                return false;
            }

            Gender? gender = null;
            if (this.def.pawnFixedGender != Gender.None)
            {
                gender = def.pawnFixedGender;
            }

            PawnKindDef pawnKindDef;
            var ofPlayer = Faction.OfPlayer;
            var list = (from def in DefDatabase<PawnKindDef>.AllDefs
                where def.race == ofPlayer.def.basicMemberKind.race && def.defName.Contains("BKRescuer")
                select def).ToList();
            if (list.Count > 0)
            {
                pawnKindDef = list.RandomElement();
            }
            else
            {
                list = (from def in DefDatabase<PawnKindDef>.AllDefs
                    where def.defName.Contains("BKRescuer")
                    select def).ToList();
                pawnKindDef = list.RandomElement();
            }

            pawnKindDef.defaultFactionType = ofPlayer.def;
            var pawnMustBeCapableOfViolence = this.def.pawnMustBeCapableOfViolence;
            var gender2 = gender;
            var pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, ofPlayer,
                PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, pawnMustBeCapableOfViolence, 20f,
                false, true, true, false, false, false, false, false, 0, 0, null, 1, null, null, null, null, null, null,
                null, gender2));
            GenSpawn.Spawn(pawn, loc, map);
            TaggedString text = GenText.AdjustedFor(this.def.letterText.Formatted(pawn.Named("PAWN")), pawn);
            TaggedString text2 = GenText.AdjustedFor(this.def.letterLabel.Formatted(pawn.Named("PAWN")), pawn);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref text2, pawn);
            Find.LetterStack.ReceiveLetter(text2, text, LetterDefOf.PositiveEvent, pawn);
            return true;
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x0000637C File Offset: 0x0000457C
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith(c => map.reachability.CanReachColony(c) && !c.Fogged(map), map,
                CellFinder.EdgeRoadChance_Neutral, out cell);
        }
    }
}
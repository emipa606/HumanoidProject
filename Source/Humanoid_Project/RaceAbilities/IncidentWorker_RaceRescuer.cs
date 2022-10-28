using System.Linq;
using RimWorld;
using Verse;

namespace RaceAbilities;

public class IncidentWorker_RaceRescuer : IncidentWorker
{
    private const float RelationWithColonistWeight = 20f;

    protected override bool CanFireNowSub(IncidentParms parms)
    {
        if (!base.CanFireNowSub(parms))
        {
            return false;
        }

        var map = (Map)parms.target;
        return TryFindEntryCell(map, out _);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        var map = (Map)parms.target;
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
            PawnGenerationContext.NonPlayer, -1, true, false, false, true, pawnMustBeCapableOfViolence, 20f,
            false, true, allowFood: true, allowAddictions: false, inhabitant: false, certainlyBeenInCryptosleep: false,
            forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0,
            biocodeApparelChance: 0, extraPawnForExtraRelationChance: null, relationWithExtraPawnChanceFactor: 1,
            validatorPreGear: null, validatorPostGear: null, forcedTraits: null, prohibitedTraits: null,
            minChanceToRedressWorldPawn: null, fixedBiologicalAge: null,
            fixedChronologicalAge: null, fixedGender: gender2));
        GenSpawn.Spawn(pawn, loc, map);
        TaggedString text = GenText.AdjustedFor(this.def.letterText.Formatted(pawn.Named("PAWN")), pawn);
        TaggedString text2 = GenText.AdjustedFor(this.def.letterLabel.Formatted(pawn.Named("PAWN")), pawn);
        PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref text2, pawn);
        Find.LetterStack.ReceiveLetter(text2, text, LetterDefOf.PositiveEvent, pawn);
        return true;
    }

    private bool TryFindEntryCell(Map map, out IntVec3 cell)
    {
        return CellFinder.TryFindRandomEdgeCellWith(c => map.reachability.CanReachColony(c) && !c.Fogged(map), map,
            CellFinder.EdgeRoadChance_Neutral, out cell);
    }
}
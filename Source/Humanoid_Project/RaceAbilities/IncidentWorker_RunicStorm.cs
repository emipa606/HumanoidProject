using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RaceAbilities;

public class IncidentWorker_RunicStorm : IncidentWorker
{
    protected override bool CanFireNowSub(IncidentParms parms)
    {
        return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.RunicStorm);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        var map = (Map)parms.target;
        var num = Mathf.RoundToInt(def.durationDays.RandomInRange * 60000f);
        var gameCondition_RunicStorm =
            (GameCondition_RunicStorm)GameConditionMaker.MakeCondition(GameConditionDefOf.RunicStorm, num);
        map.gameConditionManager.RegisterCondition(gameCondition_RunicStorm);
        var runicStorm = DefDatabase<IncidentDef>.GetNamed("RunicStorm");
        //base.SendStandardLetter(null, new TargetInfo(gameCondition_RunicStorm.centerLocation.ToIntVec3, map, false), new NamedArgument());
        SendStandardLetter(runicStorm.letterLabel, runicStorm.letterText, def.letterDef, parms,
            new TargetInfo(gameCondition_RunicStorm.centerLocation.ToIntVec3, map), Array.Empty<NamedArgument>());
        if (map.weatherManager.curWeather.rainRate > 0.1f)
        {
            map.weatherDecider.StartNextWeather();
        }

        return true;
    }
}
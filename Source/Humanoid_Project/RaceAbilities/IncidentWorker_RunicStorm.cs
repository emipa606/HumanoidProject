using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RaceAbilities
{
	// Token: 0x0200003B RID: 59
	public class IncidentWorker_RunicStorm : IncidentWorker
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000066A3 File Offset: 0x000048A3
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.RunicStorm);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000066C4 File Offset: 0x000048C4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_RunicStorm gameCondition_RunicStorm = (GameCondition_RunicStorm)GameConditionMaker.MakeCondition(GameConditionDefOf.RunicStorm, num);
			map.gameConditionManager.RegisterCondition(gameCondition_RunicStorm);
			IncidentDef runicStorm = DefDatabase<IncidentDef>.GetNamed("RunicStorm");
			//base.SendStandardLetter(null, new TargetInfo(gameCondition_RunicStorm.centerLocation.ToIntVec3, map, false), new NamedArgument());
			base.SendStandardLetter(runicStorm.letterLabel, runicStorm.letterText, this.def.letterDef, parms, new TargetInfo(gameCondition_RunicStorm.centerLocation.ToIntVec3, map, false), Array.Empty<NamedArgument>());
			if (map.weatherManager.curWeather.rainRate > 0.1f)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}

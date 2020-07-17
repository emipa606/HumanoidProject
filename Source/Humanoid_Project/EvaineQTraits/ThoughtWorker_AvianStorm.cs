using System;
using RimWorld;
using Verse;

namespace EvaineQTraits
{
	// Token: 0x0200002E RID: 46
	public class ThoughtWorker_AvianStorm : ThoughtWorker
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00005364 File Offset: 0x00003564
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			if (!p.RaceProps.Humanlike)
			{
				return ThoughtState.Inactive;
			}
			if (!p.story.traits.HasTrait(TraitDefOfEvaineQ.AvianStorm))
			{
				return ThoughtState.Inactive;
			}
			if (p.Map.weatherManager.RainRate < 0.2f)
			{
				return ThoughtState.Inactive;
			}
			if (p.Map.weatherManager.SnowRate > 0.2f)
			{
				return ThoughtState.Inactive;
			}
			if (p.Map.weatherManager.CurWindSpeedFactor < 0.2f)
			{
				return ThoughtState.Inactive;
			}
			if (p.Position.Roofed(p.Map))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}
	}
}

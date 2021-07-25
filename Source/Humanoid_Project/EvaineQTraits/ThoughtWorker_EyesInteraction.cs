using RimWorld;
using Verse;

namespace EvaineQTraits
{
    // Token: 0x0200002C RID: 44
    public class ThoughtWorker_EyesInteraction : ThoughtWorker
    {
        // Token: 0x0600008C RID: 140 RVA: 0x000052B4 File Offset: 0x000034B4
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

            if (!p.story.traits.HasTrait(TraitDefOfEvaineQ.EyesInteractive))
            {
                return ThoughtState.Inactive;
            }

            var unused = p.Position;
            if (!p.Awake())
            {
                return ThoughtState.Inactive;
            }

            if (p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Dark)
            {
                return ThoughtState.ActiveAtStage(0);
            }

            if (p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Overlit)
            {
                return ThoughtState.ActiveAtStage(2);
            }

            return ThoughtState.ActiveAtStage(1);
        }
    }
}
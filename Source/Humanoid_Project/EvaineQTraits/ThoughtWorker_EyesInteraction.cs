using RimWorld;
using Verse;

namespace EvaineQTraits;

public class ThoughtWorker_EyesInteraction : ThoughtWorker
{
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

        return p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Dark
            ? ThoughtState.ActiveAtStage(0)
            : ThoughtState.ActiveAtStage(p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Overlit ? 2 : 1);
    }
}
using RimWorld;
using Verse.AI;

namespace EvaineQMentalWorker;

public class MentalState_StormMark : MentalState
{
    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }
}
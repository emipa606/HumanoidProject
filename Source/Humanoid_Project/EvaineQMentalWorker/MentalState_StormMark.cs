using System;
using RimWorld;
using Verse.AI;

namespace EvaineQMentalWorker
{
	// Token: 0x02000033 RID: 51
	public class MentalState_StormMark : MentalState
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00005632 File Offset: 0x00003832
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace EvaineQBionics
{
	// Token: 0x0200002A RID: 42
	public class JobDriver_BlueVomit : JobDriver
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000050E7 File Offset: 0x000032E7
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000050E9 File Offset: 0x000032E9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005103 File Offset: 0x00003303
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005106 File Offset: 0x00003306
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				do
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						goto IL_6B;
					}
				}
				while (!c.InBounds(this.pawn.Map) || !c.Standable(this.pawn.Map));
				goto IL_77;
				IL_6B:
				c = this.pawn.Position;
				IL_77:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			};
			toil.tickAction = delegate()
			{
				if (this.ticksLeft % 150 == 149)
				{
					FilthMaker.TryMakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_BlueVomit, this.pawn.LabelIndefinite(), 1);
					if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
					{
						this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(EffecterDefOf.Blue_Vomit, TargetIndex.A);
			toil.PlaySustainerOrSound(() => SoundDefOf.Vomit);
			yield return toil;
			yield break;
		}

		// Token: 0x04000043 RID: 67
		private int ticksLeft;
	}
}

using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceWeapons
{
    // Token: 0x02000036 RID: 54
    public class Projectile_HeatImpact : Projectile
    {
        // Token: 0x04000053 RID: 83
        public CompFutureDamage compED;

        // Token: 0x04000059 RID: 89
        public float drawingIntensity;

        // Token: 0x04000056 RID: 86
        public Matrix4x4 drawingMatrix;

        // Token: 0x04000058 RID: 88
        public Vector3 drawingPosition;

        // Token: 0x04000057 RID: 87
        public Vector3 drawingScale;

        // Token: 0x0400005A RID: 90
        public Material drawingTexture;

        // Token: 0x04000052 RID: 82
        public Thing hitThing;

        // Token: 0x04000055 RID: 85
        public Material postFiringTexture;

        // Token: 0x04000054 RID: 84
        public Material preFiringTexture;

        // Token: 0x04000051 RID: 81
        public int tickCounter;

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x00005716 File Offset: 0x00003916
        public ThingDef_RaceWeapons Props => def as ThingDef_RaceWeapons;

        // Token: 0x0600009F RID: 159 RVA: 0x000056EF File Offset: 0x000038EF
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            drawingTexture = def.DrawMatSingle;
            compED = GetComp<CompFutureDamage>();
        }

        // Token: 0x060000A1 RID: 161 RVA: 0x00005723 File Offset: 0x00003923
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickCounter, "tickCounter");
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00005740 File Offset: 0x00003940
        public override void Tick()
        {
            if (tickCounter == 0)
            {
                PerformPreFiringTreatment();
            }

            if (tickCounter < Props.TickOffset)
            {
                GetPreFiringDrawingParameters();
            }
            else
            {
                if (tickCounter == Props.TickOffset)
                {
                    Fire();
                }

                GetPostFiringDrawingParameters();
            }

            if (tickCounter == Props.TickOffset + Props.TickOffsetSecond)
            {
                base.Tick();
                Destroy();
            }

            if (launcher is Pawn pawn)
            {
                var stances = pawn.stances;
                if (stances != null && (!(stances.curStance is Stance_Busy) || pawn.Dead))
                {
                    Destroy();
                }
            }

            tickCounter++;
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x00005804 File Offset: 0x00003A04
        public virtual void PerformPreFiringTreatment()
        {
            DetermineImpactExactPosition();
            var vector = (destination - origin).normalized * 0.9f;
            drawingScale = new Vector3(1f, 1f, (destination - origin).magnitude - vector.magnitude);
            drawingPosition = origin + (vector / 2f) + ((destination - origin) / 2f) + (Vector3.up * def.Altitude);
            drawingMatrix.SetTRS(drawingPosition, ExactRotation, drawingScale);
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x000058E4 File Offset: 0x00003AE4
        public virtual void GetPreFiringDrawingParameters()
        {
            if (Props.TickOffset != 0)
            {
                drawingIntensity = Props.DrawingOffset +
                                   ((Props.DrawingOffsetSecond - Props.DrawingOffset) * tickCounter / Props.TickOffset);
            }
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x0000593C File Offset: 0x00003B3C
        public virtual void GetPostFiringDrawingParameters()
        {
            if (Props.TickOffsetSecond != 0)
            {
                drawingIntensity = Props.DrawingOffsetThird + ((Props.DrawingOffsetFourth - Props.DrawingOffsetThird) *
                                                               ((tickCounter - (float) Props.TickOffset) /
                                                                Props.TickOffsetSecond));
            }
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x000059A4 File Offset: 0x00003BA4
        protected void DetermineImpactExactPosition()
        {
            var vector = destination - origin;
            var num = (int) vector.magnitude;
            var vector2 = vector / vector.magnitude;
            var destination1 = origin;
            var vector3 = origin;
            for (var i = 1; i <= num; i++)
            {
                vector3 += vector2;
                var intVec = vector3.ToIntVec3();
                if (!vector3.InBounds(Map))
                {
                    destination = destination1;
                    return;
                }

                if (!def.projectile.flyOverhead && def.projectile.alwaysFreeIntercept && i >= 5)
                {
                    var list = Map.thingGrid.ThingsListAt(Position);
                    foreach (var thing in list)
                    {
                        if (thing.def.Fillage == FillCategory.Full)
                        {
                            destination = intVec.ToVector3Shifted() +
                                          new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
                            hitThing = thing;
                            break;
                        }

                        if (thing.def.category != ThingCategory.Pawn)
                        {
                            continue;
                        }

                        var pawn = thing as Pawn;
                        var num2 = 0.45f;
                        if (pawn is {Downed: true})
                        {
                            num2 *= 0.1f;
                        }

                        var num3 = (ExactPosition - origin).MagnitudeHorizontal();
                        if (num3 < 4f)
                        {
                            num2 *= 0f;
                        }
                        else if (num3 < 7f)
                        {
                            num2 *= 0.5f;
                        }
                        else if (num3 < 10f)
                        {
                            num2 *= 0.75f;
                        }

                        if (pawn == null)
                        {
                            continue;
                        }

                        num2 *= pawn.RaceProps.baseBodySize;
                        if (!(Rand.Value < num2))
                        {
                            continue;
                        }

                        destination = intVec.ToVector3Shifted() +
                                      new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
                        hitThing = pawn;
                        break;
                    }
                }

                destination1 = vector3;
            }
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00005BE4 File Offset: 0x00003DE4
        public virtual void Fire()
        {
            ApplyDamage(hitThing);
        }

        // Token: 0x060000A8 RID: 168 RVA: 0x00005BF2 File Offset: 0x00003DF2
        protected void ApplyDamage(Thing hitThing)
        {
            if (hitThing != null)
            {
                Impact(hitThing);
                return;
            }

            ImpactSomething();
        }

        // Token: 0x060000A9 RID: 169 RVA: 0x00005C08 File Offset: 0x00003E08
        private void ImpactSomething()
        {
            if (def.projectile.flyOverhead)
            {
                var roofDef = Map.roofGrid.RoofAt(Position);
                if (roofDef != null)
                {
                    if (roofDef.isThickRoof)
                    {
                        def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(Position, Map));
                        Destroy();
                        return;
                    }

                    if (Position.GetEdifice(Map) == null || Position.GetEdifice(Map).def.Fillage != FillCategory.Full)
                    {
                        RoofCollapserImmediate.DropRoofInCells(Position, Map);
                    }
                }
            }

            if (!usedTarget.HasThing || !CanHit(usedTarget.Thing))
            {
                var list = new List<Thing>();
                list.Clear();
                var thingList = Position.GetThingList(Map);
                foreach (var thing in thingList)
                {
                    if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn ||
                         thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) &&
                        CanHit(thing))
                    {
                        list.Add(thing);
                    }
                }

                list.Shuffle();
                for (var j = 0; j < list.Count; j++)
                {
                    var thing2 = list[j];
                    float num;
                    if (thing2 is Pawn pawn)
                    {
                        num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
                        if (pawn.GetPosture() != PawnPosture.Standing &&
                            (origin - destination).MagnitudeHorizontalSquared() >= 20.25f)
                        {
                            num *= 0.2f;
                        }

                        if (launcher != null && pawn.Faction != null && launcher.Faction != null &&
                            !pawn.Faction.HostileTo(launcher.Faction))
                        {
                            num *= VerbUtility.InterceptChanceFactorFromDistance(origin, Position);
                        }
                    }
                    else
                    {
                        num = 1.5f * thing2.def.fillPercent;
                    }

                    if (!Rand.Chance(num))
                    {
                        continue;
                    }

                    Impact(list.RandomElement());
                    return;
                }

                Impact(null);
                return;
            }

            if (usedTarget.Thing is Pawn pawn2 && pawn2.GetPosture() != PawnPosture.Standing &&
                (origin - destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.2f))
            {
                Impact(null);
                return;
            }

            Impact(usedTarget.Thing);
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00005EDC File Offset: 0x000040DC
        protected override void Impact(Thing hitThing)
        {
            var map = Map;
            base.Impact(hitThing);
            if (hitThing != null)
            {
                var damageAmount = def.projectile.GetDamageAmount(DamageAmount);
                var thingDef = equipmentDef;
                var armorPenetration = def.projectile.GetArmorPenetration(ArmorPenetration);
                var dinfo = new DamageInfo(def.projectile.damageDef, damageAmount, armorPenetration,
                    ExactRotation.eulerAngles.y, launcher, null, thingDef);
                hitThing.TakeDamage(dinfo);
                if (hitThing is not Pawn pawn || pawn.Downed || !(Rand.Value < compED.chanceToProc))
                {
                    return;
                }

                FleckMaker.ThrowMicroSparks(destination, Map);
                hitThing.TakeDamage(new DamageInfo(DefDatabase<DamageDef>.GetNamed(compED.damageDef),
                    compED.damageAmount, armorPenetration, ExactRotation.eulerAngles.y, launcher));
            }
            else
            {
                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(Position, map));
                MoteMaker.MakeStaticMote(ExactPosition, map, ThingDefOf.LaserMoteWorker, 0.5f);
                ThrowMicroSparksRed(ExactPosition, Map);
            }
        }

        // Token: 0x060000AB RID: 171 RVA: 0x00006044 File Offset: 0x00004244
        public override void Draw()
        {
            Comps_PostDraw();
            Graphics.DrawMesh(MeshPool.plane10, drawingMatrix,
                FadedMaterialPool.FadedVersionOf(drawingTexture, drawingIntensity), 0);
        }

        // Token: 0x060000AC RID: 172 RVA: 0x00006070 File Offset: 0x00004270
        public static void ThrowMicroSparksRed(Vector3 loc, Map map)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }

            Rand.PushState();
            var moteThrown = (MoteThrown) ThingMaker.MakeThing(ThingDef.Named("LaserImpactThing"));
            moteThrown.Scale = Rand.Range(1f, 1.2f);
            moteThrown.rotationRate = Rand.Range(-12f, 12f);
            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocity(Rand.Range(35, 45), 1.2f);
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
            Rand.PopState();
        }
    }
}
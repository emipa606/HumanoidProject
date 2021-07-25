using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WeatherIsolated
{
    // Token: 0x02000014 RID: 20
    [StaticConstructorOnStartup]
    public class WeatherEvent_EmpStorm : WeatherEvent_LightningFlash
    {
        // Token: 0x04000024 RID: 36
        private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt");

        // Token: 0x04000023 RID: 35
        private Mesh boltMesh;

        // Token: 0x04000022 RID: 34
        private IntVec3 strikeLoc = IntVec3.Invalid;

        // Token: 0x06000040 RID: 64 RVA: 0x000037A0 File Offset: 0x000019A0
        public WeatherEvent_EmpStorm(Map map) : base(map)
        {
        }

        // Token: 0x06000041 RID: 65 RVA: 0x000037B4 File Offset: 0x000019B4
        public WeatherEvent_EmpStorm(Map map, IntVec3 forcedStrikeLoc) : base(map)
        {
            strikeLoc = forcedStrikeLoc;
        }

        // Token: 0x06000042 RID: 66 RVA: 0x000037D0 File Offset: 0x000019D0
        public override void FireEvent()
        {
            base.FireEvent();
            if (!strikeLoc.IsValid)
            {
                strikeLoc = CellFinderLoose.RandomCellWith(sq => sq.Standable(map) && !map.roofGrid.Roofed(sq), map);
            }

            boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            if (!strikeLoc.Fogged(map))
            {
                GenExplosion.DoExplosion(strikeLoc, map, 8f, DamageDefOf.EMP, null);
                var loc = strikeLoc.ToVector3Shifted();
                for (var i = 0; i < 4; i++)
                {
                    MoteMaker.ThrowEMPSmoke(loc, map, 2f);
                    MoteMaker.ThrowEMPMicroSparks(loc, map);
                    MoteMaker.ThrowEMPLightningGlow(loc, map, 4.5f);
                }
            }

            var info = SoundInfo.InMap(new TargetInfo(strikeLoc, map));
            SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }

        // Token: 0x06000043 RID: 67 RVA: 0x000038D8 File Offset: 0x00001AD8
        public override void WeatherEventDraw()
        {
            Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller),
                Quaternion.identity, FadedMaterialPool.FadedVersionOf(LightningMat, LightningBrightness), 0);
        }
    }
}
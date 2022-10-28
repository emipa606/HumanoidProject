using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WeatherIsolated;

[StaticConstructorOnStartup]
public class WeatherEvent_EmpStorm : WeatherEvent_LightningFlash
{
    private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt");

    private Mesh boltMesh;

    private IntVec3 strikeLoc = IntVec3.Invalid;

    public WeatherEvent_EmpStorm(Map map) : base(map)
    {
    }

    public WeatherEvent_EmpStorm(Map map, IntVec3 forcedStrikeLoc) : base(map)
    {
        strikeLoc = forcedStrikeLoc;
    }

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

    public override void WeatherEventDraw()
    {
        Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller),
            Quaternion.identity, FadedMaterialPool.FadedVersionOf(LightningMat, LightningBrightness), 0);
    }
}
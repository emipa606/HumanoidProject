using System.Reflection;
using HarmonyLib;
using Verse;

namespace DedicatedSupport;

public class EnchantedPlus : Mod
{
    public EnchantedPlus(ModContentPack pack) : base(pack)
    {
        new Harmony("EvaineQ.HumanoidProject").PatchAll(Assembly.GetExecutingAssembly());
    }
}
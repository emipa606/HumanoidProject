using System.Reflection;
using HarmonyLib;
using Verse;

namespace DedicatedSupport
{
    // Token: 0x02000015 RID: 21
    public class EnchantedPlus : Mod
    {
        // Token: 0x06000046 RID: 70 RVA: 0x00003940 File Offset: 0x00001B40
        public EnchantedPlus(ModContentPack pack) : base(pack)
        {
            new Harmony("EvaineQ.HumanoidProject").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
namespace DedicatedSupport;


//[HarmonyPatch(typeof(ModContentLoader<Texture2D>), "LoadPNG", new Type[]
//{
//	typeof(string)
//})]
//[StaticConstructorOnStartup]
//internal class SupportSpecific
//{
//	private static bool Prefix(string filePath, ref Texture2D __result)
//	{
//		Texture2D texture2D = null;
//		if (File.Exists(filePath))
//		{
//			byte[] array = File.ReadAllBytes(filePath);
//			texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
//			texture2D.LoadRawTextureData(array);
//			texture2D.name = Path.GetFileNameWithoutExtension(filePath);
//			texture2D.filterMode = FilterMode.Trilinear;
//			texture2D.Apply(true, true);
//		}
//		__result = texture2D;
//		return false;
//	}
//}
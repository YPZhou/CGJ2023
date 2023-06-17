using UnityEditor;

namespace CGJ2023.Editor
{
	public class AssetDatabaseRefresher
	{
		[MenuItem("CGJ2023/Refresh Assets %#Z")]
		public static void RefreshAssetDatabase()
		{
			AssetDatabase.Refresh();
		}
	}
}

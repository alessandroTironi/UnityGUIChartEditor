using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System.Linq;

namespace NothingButTheGame.ChartEditor
{
	internal static class GUIChartEditorPostBuildTask 
	{
		[PostProcessBuild(0)]		
		public static void DeleteTexturesFromStreamingAssetsFolder(BuildTarget target, string pathToBuiltProject)
		{
			// Simply deletes the GUIChartEditor folder from streaming assets, since it contains
			// resources that are only used in the editor.
			foreach (string dir in Directory.GetDirectories(Path.GetDirectoryName(pathToBuiltProject))
				.Where(s => s.EndsWith(Path.Combine(Path.DirectorySeparatorChar.ToString(), "StreamingAssets"))))
				Directory.Delete(dir, true);
		}
	}
}

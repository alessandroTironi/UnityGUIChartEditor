using UnityEngine;
using UnityEditor;
using NothingButTheGame.ChartEditor;

[CustomEditor(typeof(ExampleMonobehaviour))]
public class ExampleCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		float minX = serializedObject.FindProperty("minX").floatValue;
		float maxX = serializedObject.FindProperty("maxX").floatValue;
		float minY = serializedObject.FindProperty("minY").floatValue;
		float maxY = serializedObject.FindProperty("maxY").floatValue;
		GUIChartEditorOptions.Origins originType = 
			serializedObject.FindProperty("useTopLeftOrigin").boolValue ?
			GUIChartEditorOptions.Origins.TopLeft : GUIChartEditorOptions.Origins.BottomLeft;
		Color axesColor = serializedObject.FindProperty("axesColor").colorValue;
		Color gridColor = serializedObject.FindProperty("gridColor").colorValue;
		float gridCellHorSize = serializedObject.FindProperty("gridCellHorizontalSize").floatValue;
		float gridCellVerSize = serializedObject.FindProperty("gridCellVerticalSize").floatValue;

		GUIChartEditor.BeginChart(10, 100, 100, 100, Color.black,
			GUIChartEditorOptions.ChartBounds(minX, maxX, minY, maxY),
			GUIChartEditorOptions.SetOrigin(originType),
			GUIChartEditorOptions.ShowAxes(axesColor),
			GUIChartEditorOptions.ShowGrid(gridCellHorSize, gridCellVerSize, gridColor));

		// Draws lines
		SerializedProperty[] functions = 
			new SerializedProperty[serializedObject.FindProperty("functions").arraySize];
		for (int i = 0; i < functions.Length; i++)
		{
			functions[i] = serializedObject.FindProperty("functions").GetArrayElementAtIndex(i);
			Vector2[] points = new Vector2[functions[i].FindPropertyRelative("points").arraySize];
			Color functionColor = functions[i].FindPropertyRelative("funColor").colorValue;
			for (int j = 0; j < points.Length; j++)
				points[j] = functions[i].FindPropertyRelative("points").GetArrayElementAtIndex(j).vector2Value;
			GUIChartEditor.PushLineChart(points, functionColor);
		}

		GUIChartEditor.EndChart();
	}
}

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

		GUIChartEditor.BeginChart(10, 100, 10, 100, Color.black,
			GUIChartEditorOptions.ChartBounds(minX, maxX, minY, maxY),
			GUIChartEditorOptions.SetOrigin(originType),
			GUIChartEditorOptions.ShowAxes(axesColor),
			GUIChartEditorOptions.ShowGrid(gridCellHorSize, gridCellVerSize, gridColor));
		GUIChartEditor.EndChart();
	}
}

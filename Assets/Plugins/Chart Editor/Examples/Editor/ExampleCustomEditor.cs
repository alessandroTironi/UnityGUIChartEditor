using UnityEngine;
using UnityEditor;

namespace Syrus.Plugins.ChartEditor.Examples
{
    [CustomEditor( typeof( ExampleMonobehaviour ) )]
    public class ExampleCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            float minX = serializedObject.FindProperty( "minX" ).floatValue;
            float maxX = serializedObject.FindProperty( "maxX" ).floatValue;
            float minY = serializedObject.FindProperty( "minY" ).floatValue;
            float maxY = serializedObject.FindProperty( "maxY" ).floatValue;
            ChartOrigins originType =
                serializedObject.FindProperty( "useTopLeftOrigin" ).boolValue ?
                ChartOrigins.TopLeft : ChartOrigins.BottomLeft;
            Color bgColor = serializedObject.FindProperty( "backgroundColor" ).colorValue;
            Color axesColor = serializedObject.FindProperty( "axesColor" ).colorValue;
            Color gridColor = serializedObject.FindProperty( "gridColor" ).colorValue;
            float gridCellHorSize = serializedObject.FindProperty( "gridCellHorizontalSize" ).floatValue;
            float gridCellVerSize = serializedObject.FindProperty( "gridCellVerticalSize" ).floatValue;

            GUILayout.BeginHorizontal( EditorStyles.helpBox );
            GUIChartEditor.BeginChart( 10, 100, 100, 100, bgColor,
                GUIChartEditorOptions.ChartBounds( minX, maxX, minY, maxY ),
                GUIChartEditorOptions.SetOrigin( originType ),
                GUIChartEditorOptions.ShowAxes( axesColor ),
                GUIChartEditorOptions.ShowGrid( gridCellHorSize, gridCellVerSize, gridColor, true )
                /*,GUIChartEditorOptions.ShowLabels("0.##", 1f, 1f, -0.1f, 1f, -0.075f, 1f)*/);

            // Draws lines
            SerializedProperty[] functions =
                new SerializedProperty[ serializedObject.FindProperty( "functions" ).arraySize ];
            for( int i = 0; i < functions.Length; i++ )
            {
                functions[ i ] = serializedObject.FindProperty( "functions" ).GetArrayElementAtIndex( i );
                Vector2[] points = new Vector2[ functions[ i ].FindPropertyRelative( "points" ).arraySize ];
                Color functionColor = functions[ i ].FindPropertyRelative( "funColor" ).colorValue;
                for( int j = 0; j < points.Length; j++ )
                    points[ j ] = functions[ i ].FindPropertyRelative( "points" ).GetArrayElementAtIndex( j ).vector2Value;
                GUIChartEditor.PushLineChart( points, functionColor );
            }

            // Additional test: draws a lambda-defined function.
            GUIChartEditor.PushFunction( x => x * x * x, -10f, 10f, new Color( 0f, 1f, 0f, 0.5f ) );

            GUIChartEditor.EndChart();
            GUILayout.EndHorizontal();
        }
    }
}
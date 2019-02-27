using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NothingButTheGame.ChartEditor
{
	public class GUIChartEditor
	{
		/// <summary>
		/// The current instance of the drawed chart.
		/// </summary>
		public static ChartInstance CurrentChart { get; private set; }

		/// <summary>
		/// Starts drawing a new chart.
		/// </summary>
		/// <param name="layoutRect">The <see cref="Rect"/> used as layout.</param>
		/// <param name="backgroundColor">The color used for the background.</param>
		/// <param name="options">A set of options for customizing the chart.</param>
		public static void BeginChart(Rect layoutRect, Color backgroundColor, params ChartOption[] options)
		{
			GUILayout.BeginHorizontal(EditorStyles.helpBox);

			// Creates new instance of a chart.
			CurrentChart = new ChartInstance(layoutRect, backgroundColor);

			// Applies options.
			var optionsList = new List<ChartOption>(options);
			optionsList.Sort(new ChartOptionComparer());
			foreach (var option in optionsList)
				option.ApplyOption();
		}

		/// <summary>
		/// Draws a rectangular clip used as background for a chart in the current inspector
		/// position.
		/// </summary>
		/// <param name="minWidth">The rect minimum width.</param>
		/// <param name="maxWidth">The rect maximum width.</param>
		/// <param name="minHeight">The rect minimum height.</param>
		/// <param name="maxHeight">The rect maximum height.</param>
		/// <param name="backgroundColor">The color of the background.</param>
		/// <param name="options">A set of options to customize the chart.</param>
		public static void BeginChart(float minWidth, float maxWidth, float minHeight, float maxHeight,
			Color backgroundColor, params ChartOption[] options)
		{
			Rect layoutRect = GUILayoutUtility.GetRect(minWidth, maxWidth, minHeight, maxHeight);
			BeginChart(layoutRect, backgroundColor, options);
		}

		/// <summary>
		/// Pushes a new line chart in the current drawed chart.
		/// </summary>
		/// <param name="points">The set of points that form the graph.</param>
		/// <param name="lineColor">The color of the line.</param>
		public static void PushLineChart(Vector2[] points, Color lineColor)
		{
			ChartInstance.Line newLine;
			newLine.lineColor = lineColor;
			if (points.Length <= 1)
				return;
			newLine.points = new Vector2[points.Length + points.Length - 2];
			int j = 0;
			for (int i = 0; i < points.Length - 1; i++)
			{
				Vector2 p = CurrentChart.coordinatesProcessor(points[i].x, points[i].y);
				Vector2 p1 = CurrentChart.coordinatesProcessor(points[i + 1].x, points[i + 1].y);
				newLine.points[j++] = p;
				newLine.points[j++] = p1;
			}
			CurrentChart.lineQueue.Enqueue(newLine);
		}

		/// <summary>
		/// Pushes a point in the current drawed chart.
		/// </summary>
		/// <param name="points">The set of points that form the graph.</param>
		/// <param name="lineColor">The color of the line.</param>
		public static void PushPoint(Vector2 point, Color pointColor)
		{
			ChartInstance.Point p;
			p.pointColor = pointColor;
			p.point = CurrentChart.coordinatesProcessor(point.x, point.y);
			CurrentChart.pointQueue.Enqueue(p);
		}

		/// <summary>
		/// Terminates the graph drawing process.
		/// </summary>
		public static void EndChart()
		{
			if (Event.current.type == EventType.Repaint)
			{
				GUI.BeginClip(CurrentChart.pixelSizeRect);
				GL.PushMatrix();

				// Clear the current render buffer.
				GL.Clear(true, false, Color.black);
				Material inspectorMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
				inspectorMaterial.SetPass(0);

				// Draws the background.
				GL.Begin(GL.QUADS);
				GL.Color(Color.black);
				GL.Vertex3(0, 0, 0);
				GL.Vertex3(CurrentChart.pixelSizeRect.width, 0, 0);
				GL.Vertex3(CurrentChart.pixelSizeRect.width, CurrentChart.pixelSizeRect.height, 0);
				GL.Vertex3(0, CurrentChart.pixelSizeRect.height, 0);
				GL.End();

				// Draws the axes if needed.
				if (CurrentChart.showAxes)
				{
					ChartInstance.Line x, y;
					(x.lineColor, y.lineColor) = (CurrentChart.axesColor, CurrentChart.axesColor);
					Vector2[] xPoints = new Vector2[]
					{
						new Vector2(CurrentChart.minX, 0f),
						new Vector2(CurrentChart.maxX, 0f)
					};
					Vector2[] yPoints = new Vector2[]
					{
						new Vector2(0f, CurrentChart.minY),
						new Vector2(0f, CurrentChart.maxY)
					};
					PushLineChart(xPoints, CurrentChart.axesColor);
					PushLineChart(yPoints, CurrentChart.axesColor);
				}

				// Draws the points.
				GL.Begin(GL.LINES);
				while (CurrentChart.pointQueue.Count > 0)
				{
					ChartInstance.Point p = CurrentChart.pointQueue.Dequeue();
					GL.Color(p.pointColor);
					GL.Vertex3(p.point.x, p.point.y, 0);
					GL.Vertex3(p.point.x, p.point.y, 0);
				}
				GL.End();

				// Draws the lines.
				GL.Begin(GL.LINES);
				while (CurrentChart.lineQueue.Count > 0)
				{
					ChartInstance.Line l = CurrentChart.lineQueue.Dequeue();
					GL.Color(l.lineColor);
					foreach (Vector2 p in l.points)
						GL.Vertex3(p.x, p.y, 0);
				}

				GL.End();

				GL.PopMatrix();
				GUI.EndClip();
			}

			GUILayout.EndHorizontal();
		}
	}
}
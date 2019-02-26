using UnityEngine;

namespace NothingButTheGame.ChartEditor
{
	public static class GUIChartEditorOptions
	{
		/// <summary>
		/// Sets the rect limits of the graph.
		/// </summary>
		/// <param name="minX"></param>
		/// <param name="maxX"></param>
		/// <param name="minY"></param>
		/// <param name="maxY"></param>
		/// <returns></returns>
		public static ChartOption ChartBounds(float minX, float maxX, float minY, float maxY)
		{
			return new ChartBoundsOption(minX, maxX, minY, maxY);
		}

		/// <summary>
		/// Shows the axes of the reference frame.
		/// </summary>
		/// <param name="axesColor">The color of the axes.</param>
		public static ChartOption ShowAxes(Color axesColor)
		{
			return new ShowAxesOption(axesColor);
		}

		/// <summary>
		/// Sets the origin and direction of the axes.
		/// </summary>
		/// <param name="originType">The origin of the axes.</param>
		public static ChartOption SetOrigin(Origins originType)
		{
			return new SetOriginOption(originType);
		}

		/// <summary>
		/// Shows the grid with a provided cell size.
		/// </summary>
		/// <param name="cellWidth">The horizontal size (in user space) of cells.</param>
		/// <param name="cellHeight">The vertical size (in user space) of cells.</param>
		/// <param name="gridColor">The color of grid lines.</param>
		/// <returns></returns>
		public static ChartOption ShowGrid(float cellWidth, float cellHeight, Color gridColor)
		{
			return new ShowGridOption(cellWidth, cellHeight, gridColor);
		}

		/// <summary>
		/// Sets a new proportion for the rect.
		/// </summary>
		private class ChartBoundsOption : ChartOption
		{
			/// <summary>
			/// Scale rate values.
			/// </summary>
			float minX, maxX, minY, maxY;
			public ChartBoundsOption(float minX, float maxX, float minY, float maxY)
			{
				(this.minX, this.maxX, this.minY, this.maxY) = (minX, maxX, minY, maxY);
			}

			public override void ApplyOption()
			{
				GUIChartEditor.CurrentChart.minX = minX;
				GUIChartEditor.CurrentChart.maxX = maxX;
				GUIChartEditor.CurrentChart.minY = minY;
				GUIChartEditor.CurrentChart.maxY = maxY;
				GUIChartEditor.CurrentChart.origin =
					GUIChartEditor.CurrentChart.coordinatesProcessor(Mathf.Abs(minX), Mathf.Abs(minY));
				GUIChartEditor.CurrentChart.userDefinedRect = new Rect(minX, minY,
					Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY));
			}
		}

		/// <summary>
		/// Shows axes marking the origin of the reference frame.
		/// </summary>
		private class ShowAxesOption : ChartOption
		{
			private Color axesColor;
			public ShowAxesOption(Color axesColor)
			{
				this.axesColor = axesColor;
			}

			public override void ApplyOption()
			{
				GUIChartEditor.CurrentChart.showAxes = true;
				GUIChartEditor.CurrentChart.axesColor = axesColor;
			}
		}

		/// <summary>
		/// Allows to set the origin of the reference frame in the chart.
		/// </summary>
		private class SetOriginOption : ChartOption
		{
			Origins originType;
			public SetOriginOption(Origins originType)
			{
				this.originType = originType;
			}

			public override void ApplyOption()
			{
				if (originType == Origins.BottomLeft)
					GUIChartEditor.CurrentChart.coordinatesProcessor =
						GUIChartEditor.CurrentChart.BottomLeftOrigin;
				else if (originType == Origins.TopLeft)
					GUIChartEditor.CurrentChart.coordinatesProcessor =
						GUIChartEditor.CurrentChart.TopLeftOrigin;
			}
		}


		private class ShowGridOption : ChartOption
		{
			private float hSize, vSize;
			private Color gridColor;

			public ShowGridOption(float hSize, float vSize, Color gridColor)
			{
				this.hSize = hSize;
				this.vSize = vSize;
				this.gridColor = gridColor;
			}

			public override void ApplyOption()
			{
				ChartInstance chart = GUIChartEditor.CurrentChart;

				// Draws horizontal lines.
				float x = chart.minX + (Mathf.Abs(chart.minX) % hSize);
				while (x < chart.maxX)
				{
					GUIChartEditor.PushLineChart(new Vector2[]
					{
					new Vector2(x, chart.minY),
					new Vector2(x, chart.maxY)
					}, gridColor);
					x += hSize;
				}

				// Draws vertical lines.
				float y = chart.minY + (Mathf.Abs(chart.minY) % vSize);
				while (y < chart.maxY)
				{
					GUIChartEditor.PushLineChart(new Vector2[]
					{
					new Vector2(chart.minX, y),
					new Vector2(chart.maxX, y)
					}, gridColor);
					y += vSize;
				}
			}
		}

		/// <summary>
		/// The available origins for the reference frame.
		/// </summary>
		public enum Origins
		{
			TopLeft,
			BottomLeft
		}
	}

	/// <summary>
	/// An option for <see cref="GUIChartEditor"/>.
	/// </summary>
	public abstract class ChartOption
	{
		public abstract void ApplyOption();
	}
}


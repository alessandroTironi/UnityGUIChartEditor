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
		/// <param name="addLabels">If true then add labels at the bottom of axes.</param>
		/// <returns></returns>
		public static ChartOption ShowGrid(float cellWidth, float cellHeight, Color gridColor, 
			bool addLabels = false)
		{
			return new ShowGridOption(cellWidth, cellHeight, gridColor, addLabels);
		}

		/// <summary>
		/// Shows numeric labels on the chart.
		/// </summary>
		/// <param name="format">The formatting string used to stringify provided values.</param>
		/// <param name="labels">3-ples of floats representing value, X and Y of each label.</param>
		public static ChartOption ShowLabels(string format, params float[] labels)
		{
			return new ShowLabelsOption(format, labels);
		}

		/// <summary>
		/// Sets a new proportion for the rect.
		/// </summary>
		internal class ChartBoundsOption : ChartOption
		{
			/// <summary>
			/// Scale rate values.
			/// </summary>
			float minX, maxX, minY, maxY;
			public ChartBoundsOption(float minX, float maxX, float minY, float maxY) : base(0)
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
		internal class ShowAxesOption : ChartOption
		{
			private Color axesColor;
			public ShowAxesOption(Color axesColor) : base(2)
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
		internal class SetOriginOption : ChartOption
		{
			Origins originType;
			public SetOriginOption(Origins originType) : base(1)
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

		/// <summary>
		/// Shows a grid with defined cell sizes.
		/// </summary>
		internal class ShowGridOption : ChartOption
		{
			private float hSize, vSize;
			private Color gridColor;
			private bool addLabels;

			public ShowGridOption(float hSize, float vSize, Color gridColor, bool addLabels) : base(2)
			{
				this.hSize = hSize;
				this.vSize = vSize;
				this.gridColor = gridColor;
				this.addLabels = addLabels;
			}

			public override void ApplyOption()
			{
				ChartInstance chart = GUIChartEditor.CurrentChart;

				// Draws vertical lines.
				float x = chart.minX + (Mathf.Abs(chart.minX) % hSize);
				while (x < chart.maxX)
				{
					GUIChartEditor.PushLineChart(new Vector2[]
					{
						new Vector2(x, chart.minY),
						new Vector2(x, chart.maxY)
					}, gridColor);

					if (addLabels && x != 0 && x > chart.minX)
					{
						float minHeight = Mathf.Ceil(GUIChartEditor.sprites.Digits["0"].height / 2 + 2);
						float vOffset = minHeight * GUIChartEditor.CurrentChart.userDefinedRect.height
							/ GUIChartEditor.CurrentChart.pixelSizeRect.height;
						GUIChartEditor.PushValueLabel(x, x, -vOffset, "0.0#");
					}
					x += hSize;
				}

				// Draws horizontal lines.
				float y = chart.minY + (Mathf.Abs(chart.minY) % vSize);
				while (y < chart.maxY)
				{
					GUIChartEditor.PushLineChart(new Vector2[]
					{
					new Vector2(chart.minX, y),
					new Vector2(chart.maxX, y)
					}, gridColor);

					if (addLabels && y != 0 && y > chart.minY)
					{
						string label = y.ToString("0.0#").Replace(',', '.');
						float hSize = 0f;
						foreach (char c in label)
							hSize += GUIChartEditor.sprites.Digits[c.ToString()].width + 1;
						float hOffset = hSize / 2 * GUIChartEditor.CurrentChart.userDefinedRect.width
							/ GUIChartEditor.CurrentChart.pixelSizeRect.width;
						GUIChartEditor.PushValueLabel(y, -hOffset, y, "0.0#");
					}

					y += vSize;
				}
			}
		}

		internal class ShowLabelsOption : ChartOption
		{
			/// <summary>
			/// The coordinates + value of the labels to show.
			/// </summary>
			private float[] labels;

			/// <summary>
			/// The string that defines the formatting rule for labels.
			/// </summary>
			private string format;

			public ShowLabelsOption(string format, params float[] labels) : base(3)
			{
				if (labels.Length % 3 != 0)
				{
					Debug.LogError("ShowLabels requires a multiple of 3 parameters.");
					return;
				}
				this.format = format;
				this.labels = labels;
			}

			public override void ApplyOption()
			{
				for (int i = 0; i < labels.Length; i += 3)
					GUIChartEditor.PushValueLabel(labels[i], labels[i + 1], labels[i + 2], format);
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
		public int Priority { get; protected set; } = 0;

		public ChartOption(int priority)
		{
			Priority = priority;
		}

		public abstract void ApplyOption();
	}
}


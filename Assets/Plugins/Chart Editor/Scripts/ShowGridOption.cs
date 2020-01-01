/**
 * Copyright (c) 2019 Alessandro Tironi
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLRDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USER OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;

namespace Syrus.Plugins.ChartEditor
{
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
			this.hSize = Mathf.Max(0, hSize);
			this.vSize = Mathf.Max(0, vSize);
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
}

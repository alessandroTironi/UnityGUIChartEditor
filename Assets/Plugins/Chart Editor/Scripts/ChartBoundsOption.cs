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
}

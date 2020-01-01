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
}

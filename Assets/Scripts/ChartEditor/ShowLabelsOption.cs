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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syrus.Plugins.ChartEditor
{
	/// <summary>
	/// Sorts chart options according to their priority.
	/// This is necessary since the application order of chart options can
	/// break the chart.
	/// </summary>
	public class ChartOptionComparer : IComparer<ChartOption>
	{
		public int Compare(ChartOption x, ChartOption y)
		{
			return x.Priority - y.Priority;
		}
	}
}

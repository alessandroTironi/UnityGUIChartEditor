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
		public static ChartOption SetOrigin(ChartOrigins originType)
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
		/// Draws the chart to a texture.
		/// </summary>
		/// <param name="texture">The output texture.</param>
		public static ChartOption DrawToTexture(Texture2D texture)
		{
			return new DrawToTextureOption(texture);
		}
        /// <summary>
		/// Draws the chart to a texture.
		/// </summary>
		/// <param name="texture">The output texture.</param>
        /// <param name="renderSettings">The render quality settings of the output texture.</param>
		public static ChartOption DrawToTexture(Texture2D texture, TextureSettings renderSettings)
		{
			return new DrawToTextureOption(texture, renderSettings);
		}
        /// <summary>
		/// Draws the chart to a texture.
		/// </summary>
		/// <param name="texture">The output texture.</param>
        /// <param name="filtering">The filtering applied to the texture when rendered.</param>
        /// <param name="compression">How much the output texture will be compressed.</param>
		public static ChartOption DrawToTexture(Texture2D texture, FilterMode filtering, TextureCompression compression)
		{
            TextureSettings settings = new TextureSettings();
            settings.filtering = filtering;
            settings.compression = compression;

            return new DrawToTextureOption(texture, settings);
		}
	}

	/// <summary>
	/// An option for <see cref="GUIChartEditor"/>.
	/// </summary>
	public abstract class ChartOption
	{
		/// <summary>
		/// The priority of the option: options with same priority can be applied
		/// at an irrelevant order.
		/// </summary>
		public int Priority { get; protected set; } = 0;

		public ChartOption(int priority)
		{
			Priority = priority;
		}

		public abstract void ApplyOption();
	}
}


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

using System.Collections.Generic;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ChartEditorTests")]
namespace Syrus.Plugins.ChartEditor
{
	internal class ChartInstance
	{
		/// <summary>
		/// A delegate that processes a pair of coordinate according to a certain coordinate
		/// system.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <returns>The processed pair of coordinates.</returns>
		public delegate Vector2 ProcessPointCoordinates(float x, float y);

		/// <summary>
		/// The function currently used for coordinate processing.
		/// </summary>
		protected internal ProcessPointCoordinates coordinatesProcessor;

		/// <summary>
		/// The queue of lines to be drawn.
		/// </summary>
		protected internal Queue<Line> lineQueue { get; private set; } = new Queue<Line>();

		/// <summary>
		/// The queue of points to be drawn.
		/// </summary>
		protected internal Queue<Point> pointQueue { get; private set; } = new Queue<Point>();

		/// <summary>
		/// The queue of textures to be drawn.
		/// </summary>
		protected internal Queue<Texture> textureQueue { get; private set; } = new Queue<Texture>();

		/// <summary>
		/// The current layout rect.
		/// </summary>
		public Rect pixelSizeRect, userDefinedRect;

		/// <summary>
		/// The current background color.
		/// </summary>
		public Color currentBgColor;

		/// <summary>
		/// Coordinate limits.
		/// </summary>
		public float minX, maxX, minY, maxY;

		/// <summary>
		/// The origin of the reference frame.
		/// </summary>
		public Vector2 origin;

		/// <summary>
		/// If true, shows the axes
		/// </summary>
		public bool showAxes = false;

		/// <summary>
		/// The color of the displayed axes.
		/// </summary>
		public Color axesColor = Color.white;

		/// <summary>
		/// A line in the chart.
		/// </summary>
		internal struct Line
		{
			public Color lineColor;
			public Vector2[] points;
		}

		/// <summary>
		/// A point in the chart.
		/// </summary>
		internal struct Point
		{
			public Color pointColor;
			public Vector2 point;
		}

		/// <summary>
		/// A texture drawn on the chart.
		/// </summary>
		internal struct Texture
		{
			public Texture2D texture;
			public Rect rect;
		}

		/// <summary>
		/// The material used for the rendering.
		/// </summary>
		internal Material material;

		/// <summary>
		/// The texture the graph will be drawn on. If no textures are specified,
		/// the graph will be rendered on the main screen.
		/// </summary>
		internal Texture2D outputTexture = null;

        /// <summary>
        /// The render quality the output graph texture has, if we draw on it.
		/// </summary>
        internal TextureSettings outputTextureSettings;

		public ChartInstance(Rect layoutRect, Color backgroundColor)
		{
			coordinatesProcessor = BottomLeftOrigin;

			pixelSizeRect = layoutRect;
			currentBgColor = backgroundColor;
			(minX, maxX, minY, maxY) = (layoutRect.xMin, layoutRect.xMax,
				layoutRect.yMin, layoutRect.yMax);
			origin = coordinatesProcessor(Mathf.Abs(minX + layoutRect.width * 0.5f),
				Mathf.Abs(minY + layoutRect.height * 0.5f));

			// User-defined rect equals the pixel-sized one by default.
			userDefinedRect = new Rect(pixelSizeRect);
		}

		/// <summary>
		/// Simply returns the coordinates as they are provided.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <returns>The same pair of coordinates.</returns>
		internal Vector2 TopLeftOrigin(float x, float y)
		{
			Vector2 absPoint = LocalToAbsoluteReference(x, y);
			return FloatToRaw(absPoint.x, absPoint.y);
		}

		/// <summary>
		/// Reverts the Y coordinate.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>The pair of coordinates converted from TopLeft to BottomLeft
		/// orign reference frame.</returns>
		internal Vector2 BottomLeftOrigin(float x, float y)
		{
			Vector2 absPoint = LocalToAbsoluteReference(x, y);
			Vector2 rawPoint = FloatToRaw(absPoint.x, absPoint.y);

			// Reverts the y value.
			return new Vector2(rawPoint.x, pixelSizeRect.height - rawPoint.y);
		}

		/// <summary>
		/// Converts the provided coordinates from local space to clip space,
		/// i.e., from the user-defined reference frame to the clip reference frame.
		/// </summary>
		internal Vector2 LocalToAbsoluteReference(float x, float y)
		{
			return new Vector2(Mathf.Abs(minX) + x, Mathf.Abs(minY) + y);
		}

		/// <summary>
		/// Converts the provided coordinates from user-defined scale to pixel-sized
		/// scale.
		/// </summary>
		internal Vector2 FloatToRaw(float x, float y)
		{
			return new Vector2(
				x * (pixelSizeRect.width / userDefinedRect.width),
				y * (pixelSizeRect.height / userDefinedRect.height)
			);
		}
	}
}


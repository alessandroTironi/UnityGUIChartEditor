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

using NUnit.Framework;
using UnityEngine;

namespace Syrus.Plugins.ChartEditor.Tests
{
	public class ChartInstanceTest
	{

		ChartInstance chart;

		[SetUp]
		public void SetUp()
		{
			chart = new ChartInstance(new Rect(14, 100, 100, 50), Color.black);
			chart.userDefinedRect = new Rect(-0.1f, -0.2f, 1.2f, 1.3f);
			chart.minX = -0.1f;
			chart.maxX = 1.1f;
			chart.minY = -0.2f;
			chart.maxY = 1.1f;
		}

		// A Test behaves as an ordinary method
		[Test]
		public void FromLocalToAbsoluteCoordinates4Quads()
		{
			Vector2 localOrigin = Vector2.zero;
			Vector2 absOrigin = chart.LocalToAbsoluteReference(0f, 0f);
			Assert.AreEqual(0.1f, absOrigin.x);
			Assert.AreEqual(0.2f, absOrigin.y);
		}

		[Test]
		public void FloatToRawConversion()
		{
			Vector2 absPoint = new Vector2(0.1f, 0.2f);
			Vector2 rawPoint = chart.FloatToRaw(absPoint.x, absPoint.y);
			Assert.LessOrEqual(rawPoint.x, 9f);
			Assert.GreaterOrEqual(rawPoint.x, 8f);
			Assert.LessOrEqual(rawPoint.y, 8f);
			Assert.GreaterOrEqual(rawPoint.y, 7f);
		}

		[Test]
		public void BottomLeftOrigin()
		{
			Vector2 bl = chart.BottomLeftOrigin(0.1f, 0.2f);
			Assert.LessOrEqual(bl.x, 17f);
			Assert.GreaterOrEqual(bl.x, 16f);
			Assert.LessOrEqual(bl.y, 35f);
			Assert.GreaterOrEqual(bl.y, 34f);
		}

		[Test]
		public void TopLeftOrigin()
		{
			Vector2 tl = chart.TopLeftOrigin(0.1f, 0.2f);
			Assert.LessOrEqual(tl.x, 17f);
			Assert.GreaterOrEqual(tl.x, 16f);
			Assert.LessOrEqual(tl.y, 16f);
			Assert.GreaterOrEqual(tl.y, 15f);
		}

		[Test]
		/// <summary>
		/// Tests coordinate conversion when working only on top-right quad.
		/// </summary>
		public void TopRightQuadOnlyTest()
		{
			chart.userDefinedRect = new Rect(0f, 0f, 1.2f, 1.3f);
			chart.minX = 0f;
			chart.maxX = 1.2f;
			chart.minY = 0f;
			chart.maxY = 1.3f;
			chart.coordinatesProcessor = chart.TopLeftOrigin;
			Vector2 p = chart.coordinatesProcessor(0.5f, 0.5f);
			Assert.LessOrEqual(p.x, 42f);
			Assert.GreaterOrEqual(p.x, 41f);
			Assert.LessOrEqual(p.y, 20f);
			Assert.GreaterOrEqual(p.y, 19f);
		}

		[Test]
		/// <summary>
		/// Tests the coordinate conversion when working only in the top quads.
		/// </summary>
		public void TopQuadsOnlyTest()
		{
			chart.userDefinedRect = new Rect(-1f, 0f, 2f, 1f);
			chart.minX = -1f;
			chart.maxX = 1f;
			chart.minY = 0f;
			chart.maxY = 1f;
			Vector2 p = chart.TopLeftOrigin(0f, 0.6f);
			Assert.LessOrEqual(p.x, 51f);
			Assert.GreaterOrEqual(p.x, 49f);
			Assert.LessOrEqual(p.y, 31f);
			Assert.GreaterOrEqual(p.y, 29f);
		}

		[Test]
		/// <summary>
		/// Tests coordinate conversions when working only in the bottom-left
		/// quad.
		/// </summary>
		public void BottomLeftQuadOnlyTest()
		{
			chart.userDefinedRect = new Rect(-1f, -1f, 1f, 1f);
			chart.minX = -1f;
			chart.maxX = 0f;
			chart.minY = -1f;
			chart.maxY = 0f;
			Vector2 p = chart.TopLeftOrigin(-0.3f, -0.6f);
			Assert.LessOrEqual(p.x, 71f);
			Assert.GreaterOrEqual(p.x, 69f);
			Assert.LessOrEqual(p.y, 21f);
			Assert.GreaterOrEqual(p.y, 19f);
		}
	}
}

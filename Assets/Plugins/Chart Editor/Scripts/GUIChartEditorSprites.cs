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
using System.IO;
using UnityEngine;

namespace Syrus.Plugins.ChartEditor
{
	internal class GUIChartEditorSprites
	{
		/// <summary>
		/// Provides access to the texture of each single digit, including dot.
		/// </summary>
		public Dictionary<string, Texture2D> Digits { get; private set; }

		public GUIChartEditorSprites()
		{
			LoadTextures();
		}

		/// <summary>
		/// Loads digit textures from the StreamingAssets folder.
		/// </summary>
		private void LoadTextures()
		{
			Digits = new Dictionary<string, Texture2D>();

			// Loads textures in memory.
			string digitsFolder = "Chart Editor";
			for (int i = 0; i < 10; i++)
				Digits[i.ToString()] = Resources.Load<Texture2D>(Path.Combine(digitsFolder,
					"Digit_" + i.ToString()));

			// Loads the dot.
			string dotTexFile = Path.Combine(digitsFolder, "Digit_Dot");
			string minusTexFile = Path.Combine(digitsFolder, "Digit_Minus");
			Digits["."] = Resources.Load<Texture2D>(dotTexFile);
			Digits["-"] = Resources.Load<Texture2D>(minusTexFile);
		}

		/// <summary>
		/// Returns a <see cref="Rect"/> that describes the bounding box required to 
		/// contain the provided digits-only text.
		/// </summary>
		/// <param name="text">The text to be contained. Must be digits-only.</param>
		/// <param name="x">The X coordinate of the Rect.</param>
		/// <param name="y">The Y coordinate of the Rect.</param>
		/// <returns>A <see cref="Rect"/> that describes the text bounding box.</returns>
		public Rect GetRequiredRect(string text, float x, float y)
		{
			if (!float.TryParse(text, out float res))
				throw new System.ArgumentException("GetRequiredRect can only be invoked on strings " +
					"representing numbers.");
			float w = 0f;
			float h = 0f;
			foreach (char c in text)
			{
				if (!Digits.ContainsKey(c.ToString()))
				{
					Debug.LogError("Sprite for character \"" + c.ToString() + "\" not found.");
					continue;
				}
				Texture2D tex = Digits[c.ToString()];
				w += tex.width;
				h = Mathf.Max(h, tex.height);
			}
			return new Rect((int)(x - w * 0.5f),(int)(y - h * 0.5f), w, h);
		}

		/// <summary>
		/// Computes the set of textures to be drawn on the chart quad.
		/// </summary>
		/// <param name="text">The text to show.</param>
		/// <param name="pX">The X coordinate (in pixel space).</param>
		/// <param name="pY">The Y coordinate (in pixel space).</param>
		public ChartInstance.Texture[] GetTextures(string text, float pX, float pY)
		{
			Rect rect = GetRequiredRect(text, pX, pY);
			(float x, float y) = (rect.x, rect.y);
			ChartInstance.Texture[] textures = new ChartInstance.Texture[text.Length];
			int i = 0;
			foreach (char c in text)
			{
				Texture2D tex = Digits[c.ToString()];
				textures[i++] = new ChartInstance.Texture()
				{
					texture = tex,
					rect = new Rect(x, y, tex.width, tex.height)
				};
				x += tex.width;
			}
			return textures;
		}
	}
}

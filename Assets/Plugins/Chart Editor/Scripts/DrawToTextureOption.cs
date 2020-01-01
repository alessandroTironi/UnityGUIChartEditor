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
	internal class DrawToTextureOption : ChartOption
	{
		/// <summary>
		/// The texture the graph will be drawn on.
		/// </summary>
		private Texture2D texture;
        private TextureSettings texSettings;

        public DrawToTextureOption(Texture2D texture) : base(3)
		{
			this.texture = texture;
		}
		public DrawToTextureOption(Texture2D texture, TextureSettings settings) : base(3)
		{
			this.texture = texture;
            this.texSettings = settings;
		}

		public override void ApplyOption()
		{
			RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height);
			RenderTexture.active = rt;

			// By setting an output texture we tell GUIChartEditor to draw on it in
			// GUIChartEditor.EndChart().
			GUIChartEditor.CurrentChart.outputTexture = texture;

            // Tell the GUIChartEditor what render settings to apply to the texture.
            GUIChartEditor.CurrentChart.outputTextureSettings = this.texSettings;
		}
	}

    public struct TextureSettings
    {
        /// <summary>
        /// The filtering applied to the texture when rendered.
        /// </summary>
        public FilterMode filtering;

        /// <summary>
        /// Specifies how much a generated texture will be compressed.
        /// </summary>
        public TextureCompression compression;
    }

    /// <summary>
    /// Specifies how much a generated texture will be compressed.
    /// </summary>
    public enum TextureCompression : byte
    {
        /// <summary>
        /// Texture will be rendered perfectly, at the cost of slowest render speed.
        /// </summary>
        None,

        /// <summary>
        /// Texture will have a few color artifacts that are hard to notice. Best for general purposes.
        /// </summary>
        HighQuality,

        /// <summary>
        /// Texture will have a lot of color artifacts, with the benefit of fast rendering. Good for far-away objects.
        /// </summary>
        LowQuality
    }
}

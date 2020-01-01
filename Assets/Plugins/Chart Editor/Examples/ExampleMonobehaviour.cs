using System;
using UnityEngine;

namespace Syrus.Plugins.ChartEditor.Examples
{
    public class ExampleMonobehaviour : MonoBehaviour
    {
        public float minX, maxX, minY, maxY;

        public bool useTopLeftOrigin = false;

        public Color axesColor;

        public float gridCellHorizontalSize;

        public float gridCellVerticalSize;

        public Color gridColor;

        public Color backgroundColor = Color.black;

        public Function[] functions;

        [Serializable]
        public struct Function
        {
            public Color funColor;
            public Vector2[] points;
        }
    }
}
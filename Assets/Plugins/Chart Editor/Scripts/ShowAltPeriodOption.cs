using System.IO;
using UnityEngine;

namespace Syrus.Plugins.ChartEditor
{
    public class ShowAltPeriodOption : ChartOption
    {
        public ShowAltPeriodOption() : base(1)
        {
        }

        public override void ApplyOption()
        {
            string digitsFolder = "Chart Editor";
            string dotTexFile = Path.Combine(digitsFolder, "Digit_Dot_Alt");
            GUIChartEditor.sprites.Digits["."] = Resources.Load<Texture2D>(dotTexFile);
        }
    }
}
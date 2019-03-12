﻿using UnityEngine;
using Syrus.Plugins.ChartEditor;

public class RenderToQuadExample : MonoBehaviour
{

    void Start()
    {
		float f(float x)
		{
			return x * x * x;
		}

		var rend = GetComponent<MeshRenderer>();
		rend.material.hideFlags = HideFlags.HideAndDontSave;
		rend.material.shader.hideFlags = HideFlags.HideAndDontSave;
		Texture2D tex = new Texture2D(300, 150);
		GUIChartEditor.BeginChart(new Rect(0, 0, 300, 150), Color.black,
			GUIChartEditorOptions.ChartBounds(-0.5f, 1.5f, -0.25f, 1.25f),
			GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft),
			GUIChartEditorOptions.ShowAxes(Color.white),
			GUIChartEditorOptions.ShowGrid(0.25f, 0.25f, Color.grey, true),
			GUIChartEditorOptions.DrawToTexture(tex));
		GUIChartEditor.PushFunction(f, -0.5f, 1.5f, Color.green);
		GUIChartEditor.EndChart();

		rend.material.SetTexture("_MainTex", tex);
    }
}

# Unity GUI Chart Editor

This plugin allows to plot both point-by-point-defined and delegate-defined function charts either on a texture or on a custom inspector. An example of what this plugin can do is given in this picture, which shows a chart on the inspector:

![Inspector Example](doc/FullExample.png?raw=true "Full Example")

## How to use it

Each chart is defined between a <code>GUIChartEditor.BeginChart()</code> and a <code>GUIChartEditor.EndChart()</code>. Between these two invocations, you can use specific <code>GUIChartEditor</code>'s static methods for drawing specific parts of the chart.

```csharp
// Plots the f(x) = x^3 function in the editor.
GUIChartEditor.BeginChart(10, 100, 100, 100, Color.black,
    GUIChartEditorOptions.ChartBounds(-0.5f, 1.5f, -0.25f, 1.25f),
    GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft)
);
GUIChartEditor.PushFunction(x => x * x * x, -0.5f, 1.5f, Color.green);
GUIChartEditor.EndChart();
```

The <code lang="csharp">GUIChartEditor.BeginChart</code> method requires to specify either a <code lang="csharp">Rect</code> or the 4-ple <code>(minWidth, maxWidth, minHeight, maxHeight)</code>, a background color and a list of <code>ChartOption</code>s. The second choice is possible only when drawing on the inspector, since the chart editor will automatically compute the best fitting <code>Rect</code> with the given features. Chart options will be covered in the next paragraphs.

### Plotting a function

The method <code lang="csharp">GUIChartEditor.PushFunction(ChartFunction f, float min, float max, Color functionColor, float step = -1)</code> allows to draw the function defined by <code>f</code>, which is a delegate that accepts a float as parameter and returns a float, in the <code>[min, max]</code> interval. The <code>step</code> parameter defines the sampling rate followed when plotting the chart. If it is left to -1, the function value will be computed at each pixel.

```csharp
GUIChartEditor.PushFunction(x => x * x * x, -1f, 2f, Color.green);
```
![Delegate Example](doc/DelegateExample.png?raw=true "Delegate")

### Plotting a point-by-point function

The method <code>GUIChartEditor.PushLineChart(Vector2[] points, Color lineColor)</code> allows to define a polygonal function point by point and plot it in the chart.

```csharp
Vector2[] samples = new Vector2[] 
{ 
    new Vector2(0f, 0f), new Vector2(0.5f, 1f), new Vector2(1f, 0f)
};
GUIChartEditor.PushLineChart(samples, Color.red);
```
![Point-by-point Example](doc/PointByPointExample.png?raw=true "Point-by-point")

### Plotting points

Use the method <code>GUIChartEditor.PushPoint(Vector2 point, Color pointColor)</code> to draw just one point in the chart.

```csharp
GUIChartEditor.PushPoint(new Vector2(0.5f, 0.5f), Color.red);
GUIChartEditor.PushPoint(new Vector2(0.75f, 0.5f), Color.green);
GUIChartEditor.PushPoint(new Vector2(1f, 1f), Color.yellow);
```
![Point Example](doc/PointExample.png?raw=true "Points")

### Adding numerical labels

The method <code>GUIChartEditor.PushValueLabel(float value, float x, float y, string floatFormat = "0.00")</code> adds a label representing <code>value</code> in the <code>(x,y)</code> position. The float will be formatted according to the provided <code>floatFormat</code>.

```csharp
GUIChartEditor.PushValueLabel(0.95f, 1f, 0.95f);
GUIChartEditor.PushValueLabel(0.43f, 0.5f, 0.43f);
```
![Labels Example](doc/LabelsExample.png?raw=true "Labels")

## Available options

If no options are defined, the <code>PushX</code> directives invoked after <code>BeginChart</code> will interpret coordinates in a top-left origin chart where one unit has the size of a pixel. To allow a more user-friendly plotting, you can use options.

### Defining chart bounds

The <code>GUIChartEditorOptions.ChartBounds(float minX, float maxX, float minY, float maxY)</code> will define new bounds for the chart. From the application of this option, the next calls will expect coordinates in which the x is between <code>minX</code> and <code>maxX</code> and the y is between <code>minY</code> and <code>maxY</code>. This is useful if you do not want to work in pixel space.

### Changing the origin

If you want to work in a bottom-left centered chart, where the Y grows upwards, use the option <code>GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft)</code>. Otherwise, the Y of the chart will grow downwards.

### Showing axes

The option <code>GUIChartEditor.ShowAxes(Color axesColor)</code> shows the chart axes.

### Showing the grid

Use <code>GUIChartEditorOptions.ShowGrid(float cellWidth, float cellHeight, Color gridColor, bool addLabels = false)</code> to show a grid behind the chart axes (if present). You can set the size of the cells and mark <code>addLabels</code> as true if you want to show the values on the axes.

### Adding labels

To add more numerical labels, use <code>GUIChartEditorOptions.ShowLabels(string format, params float[] labels)</code>, which will add new labels with the provided <code>format</code>. <code>labels</code> is a float array whose size must be a multiple of 3, in the format: label 1 value, label 1 X, label 1 Y, label 2 value, label 2 X, label 2 Y, ..., label N value, label N X, label N Y.

### Drawing to a Texture2D

The <code>GUIChartEditorOptions.DrawToTexture(Texture2D texture)</code> will draw the chart on a Texture2D and not on the inspector. Note that the texture will be available only after the <code>EndChart</code> call.

![Draw to texture Example](doc/DrawToTextureExample.png?raw=true "Draw to texture")



As a final summary, here is the code used to generate the chart in the first example.
```csharp
GUILayout.BeginHorizontal(EditorStyles.helpBox); // comment if you render to texture
GUIChartEditor.BeginChart(10, 100, 100, 100, Color.black,
    GUIChartEditorOptions.ChartBounds(-0.5f, 1.5f, -0.5f, 1.5f),
    GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft),
    GUIChartEditorOptions.ShowAxes(Color.white),
    GUIChartEditorOptions.ShowGrid(0.25f, 0.25f, Color.grey, true)
    /* , GUIChartEditorOptions.DrawToTexture(texture) */ // un-comment to render to texture
);
Vector2[] f1 = new Vector2[] { new Vector2(0f, 0f), new Vector2(0.5f, 1f), new Vector2(1f, 0f) };
Vector2[] f2 = new Vector2[] { new Vector2(0f, 0f), new Vector2(0.75f, 1f), new Vector2(1.4f, 0f) };

GUIChartEditor.PushLineChart(f1, Color.red);
GUIChartEditor.PushPoint(new Vector2(0.5f, 1f), Color.red);
GUIChartEditor.PushLineChart(f2, Color.yellow);
GUIChartEditor.PushPoint(new Vector2(0.75f, 1f), Color.yellow);
GUIChartEditor.PushFunction(x => x * x * x, -10f, 10f, new Color(0f, 1f, 0f, 0.5f));

GUIChartEditor.EndChart();
GUILayout.EndHorizontal(); // comment if you render to texture
```

![Complete Example](doc/CompleteExample.png?raw=true "Complete")

# Contributors

@Bamboy

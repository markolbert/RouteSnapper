namespace RouteSnapper;

internal class MapViewModel
{
    public string ProjectionName { get; set; } = string.Empty;
    public string Center { get; set; } = "0,0";
    public double Heading { get; set; }
    public double Scale { get; set; }
}
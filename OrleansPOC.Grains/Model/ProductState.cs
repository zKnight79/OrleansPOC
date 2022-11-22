namespace OrleansPOC.Grains.Model;

[GenerateSerializer]
public class ProductState
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;
}

namespace OrleansPOC.GrainInterfaces;

public interface IProduct : IGrainWithStringKey
{
    Task<string> GetName();
    Task SetName(string name);
}
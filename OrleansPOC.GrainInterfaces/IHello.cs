namespace OrleansPOC.GrainInterfaces;

public interface IHello: IGrainWithGuidKey
{
    Task<string> SayHello(string name);
}

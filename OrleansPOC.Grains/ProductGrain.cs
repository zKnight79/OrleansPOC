using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using OrleansPOC.GrainInterfaces;
using OrleansPOC.Grains.Model;

namespace OrleansPOC.Grains;

public class ProductGrain : LoggableGrain<ProductGrain>, IProduct
{
    private readonly IPersistentState<ProductState> _productState;

    public ProductGrain(
        [PersistentState(stateName: "productState", storageName: "ProductStore")] IPersistentState<ProductState> productState,
        ILogger<ProductGrain> logger
    ) : base(logger)
    {
        _productState = productState;
    }

    public Task<string> GetName()
        => Task.FromResult(_productState.State.Name);

    public async Task SetName(string name)
    {
        Log($"SetName({_productState.State.Name} => {name})");

        _productState.State.Name = name;
        await _productState.WriteStateAsync();
        await Task.Delay(100);
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Log($"OnActivateAsync()");

        return base.OnActivateAsync(cancellationToken);
    }
    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        Log($"OnDeactivateAsync({reason.Description})");

        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}

# Xer.Cqrs.Extensions.Autofac
Extension for Autofac's `ContainerBuilder` to allow easy registration of command handlers and event handlers.

```csharp
public void ConfigureServices(Autofac.ContainerBuilder container)
{
    // Register all CQRS components.
    container.RegisterCqrs(typeof(CommandHandler).Assembly, 
                           typeof(EventHandler).Assembly);
}
```

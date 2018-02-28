namespace Xer.Cqrs.Extensions.Autofac
{
    public enum Lifetime
    {
        PerDependency,
        PerLifetimeScope,
        Singleton
    }
}
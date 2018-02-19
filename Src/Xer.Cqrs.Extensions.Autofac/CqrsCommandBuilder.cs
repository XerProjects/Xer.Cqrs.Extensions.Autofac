using Autofac;
using System;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsCommandBuilder : ICqrsCommandBuilder
    {
        private readonly ContainerBuilder _builder;

        internal CqrsCommandBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }

        internal ICqrsCommandBuilder AddCore()
        {
            return this;
        }

        public ICqrsCommandBuilder AddCommandHandlers(params Assembly[] assemblies)
        {
            throw new NotImplementedException();
        }

        public ICqrsCommandBuilder AddCommandHandlersByAttribute(params Assembly[] assemblies)
        {
            throw new NotImplementedException();
        }
    }
}

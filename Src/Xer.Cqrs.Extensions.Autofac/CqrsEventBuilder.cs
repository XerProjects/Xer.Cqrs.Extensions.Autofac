using Autofac;
using System;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsEventBuilder : ICqrsEventBuilder
    {
        private readonly ContainerBuilder _builder;

        internal CqrsEventBuilder (ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ICqrsEventBuilder AddEventHandlers(params Assembly[] assemblies)
        {
            throw new NotImplementedException();
        }

        public ICqrsEventBuilder AddEventHandlersByAttribute(params Assembly[] assemblies)
        {
            throw new NotImplementedException();
        }

        internal ICqrsEventBuilder AddCore()
        {

            return this;
        }
    }
}

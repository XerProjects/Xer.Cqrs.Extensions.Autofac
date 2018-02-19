using Autofac;
using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsBuilder : ICqrsBuilder
    {
        private readonly ContainerBuilder _builder;

        internal CqrsBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ICqrsBuilder AddCommands(Action<ICqrsCommandBuilder> builder)
        {
            builder(new CqrsCommandBuilder(_builder));

            return this;
        }

        public ICqrsBuilder AddEvents(Action<ICqrsEventBuilder> builder)
        {
            builder(new CqrsEventBuilder(_builder));

            return this;
        }
    }
}

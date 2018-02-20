using Autofac;
using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsBuilder : ICqrsBuilder
    {
        private readonly ContainerBuilder _builder;
        private readonly CqrsCommandBuilder _commandBuilder;
        private readonly CqrsEventBuilder _eventBuilder;

        internal CqrsBuilder(ContainerBuilder builder)
        {
            _builder = builder;
            _eventBuilder = new CqrsEventBuilder(_builder);
            _commandBuilder = new CqrsCommandBuilder(_builder);
        }

        public ICqrsBuilder AddCommands(Action<ICqrsCommandBuilder> builder = null)
        {
            if (builder == null)
            {
                return this;
            }

            builder(_commandBuilder);

            return this;
        }

        public ICqrsBuilder AddEvents(Action<ICqrsEventBuilder> builder = null)
        {
            if (builder == null)
            {
                return this;
            }

            builder(_eventBuilder);

            return this;
        }
    }
}

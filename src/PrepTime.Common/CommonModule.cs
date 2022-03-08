using Autofac;
using PrepTime.Common.Graph;
using PrepTime.Common.Util;

namespace PrepTime.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Util
            builder.RegisterType<SequentialNumberGenerator>().As<ISequentialNumberGenerator>();

            // Graph
            builder.RegisterType<DirectedAcyclicGraph>().As<IDirectedAcyclicGraph>();
            builder.RegisterType<PrepGraph>().As<IPrepGraph>();

            builder.RegisterType<Controller>().As<IController>();
        }
    }
}

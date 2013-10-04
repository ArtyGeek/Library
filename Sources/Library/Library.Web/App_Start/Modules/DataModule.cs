using Autofac;
using Library.Data;

namespace Library.Web
{
	public class DataModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<LibraryContext>().As<ILibraryContext>().InstancePerDependency();
		}
	}
}
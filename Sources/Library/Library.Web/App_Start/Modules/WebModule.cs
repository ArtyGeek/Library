using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Module = Autofac.Module;

namespace Library.Web
{
	public class WebModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule<DomainModule>();
			builder.RegisterModule<DataModule>();

			Assembly controllersAssembly = Assembly.Load("Library.Web");
			builder.RegisterControllers(controllersAssembly).InstancePerDependency();
		}
	}
}
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace Library.Web
{
	public static class ComponentsConfig
	{
		 public static void RegisterComponents()
		 {
			 ContainerBuilder builder = new ContainerBuilder();
			 builder.RegisterModule<WebModule>();
			 IContainer container = builder.Build();
			 DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		 }
	}
}
using Autofac;
using Library.Domain.Contexts;

namespace Library.Web
{
	public class DomainModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CryptographyContext>().As<ICryptographyContext>().SingleInstance();
		}
	}
}
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using ShopApp.Core.Utilities.Interceptors;
using ShopApp.Core.Utilities.Security.Jwt;
using ShopApp.Business.Concrete;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework;
using ShopApp.DataAccess.Abstract;

namespace ShopApp.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<EfProuctDal>().As<IProductDal>();

            builder.RegisterType<CategoryManager>().As<ICategoryService>();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<CartManager>().As<ICartService>();
            builder.RegisterType<EfCartDal>().As<ICartDal>();

            builder.RegisterType<OrderManager>().As<IOrderService>();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}

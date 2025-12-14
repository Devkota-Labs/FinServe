//using System.Reflection;

//namespace FinServe.Api.Extensions;

//public static class ServiceCollectionExtensions
//{
//    public static IServiceCollection RegisterModuleApis(this IServiceCollection services)
//    {
//        var assemblies = Directory
//            .GetFiles(AppContext.BaseDirectory, "*Api.dll")
//            .Select(Assembly.LoadFrom);

//        foreach (var assembly in assemblies)
//        {
//            var installerTypes = assembly
//                .GetTypes()
//                .Where(t => t.IsClass && !t.IsAbstract && typeof(IModuleApi).IsAssignableFrom(t));

//            foreach (var installer in installerTypes)
//            {
//                var instance = (IModuleApi)Activator.CreateInstance(installer)!;
//                instance.MapEndpoints(services);
//            }
//        }

//        return services;
//    }
//}

//public interface IModuleApi
//{
//    void MapEndpoints(IServiceCollection services);
//}
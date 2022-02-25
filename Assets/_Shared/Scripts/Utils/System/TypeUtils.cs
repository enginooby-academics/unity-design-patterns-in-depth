using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ? Rename to ReflectionUtils
public static class TypeUtils {
  // TIP: Use generics to pass type as argument with shorter syntax
  /// <summary>
  ///   Is this type extending from (but not) the given type?
  /// </summary>
  public static bool IsSubclassOf<T>(this Type type) => type.IsSubclassOf(typeof(T));

  // ! Doesn't work for class extending MonoBehaviour while implementing interface
  /// <summary>
  ///   Is this type extending from the given type or the given type?
  /// </summary>
  public static bool Is<T>(this Type type) => type.IsSubclassOf<T>() || type == typeof(T);

  public static bool IsListType(this Type type) => type.GetGenericTypeDefinition() == typeof(List<>);

  /// <summary>
  ///   Checks if the type implements the other type.
  /// </summary>
  public static bool Implements<T>(this Type type) {
    var interfaceTarget = typeof(T);

    if (!interfaceTarget.IsInterface) throw new InvalidOperationException();
    if (interfaceTarget == null) throw new ArgumentNullException(nameof(interfaceTarget));
    if (type == null) throw new ArgumentNullException(nameof(type));

    var interfaces = type.GetInterfaces();

    // UTIL: Compare interface types
    if (!interfaceTarget.IsGenericTypeDefinition)
      return interfaces.Any(@interface =>
        @interface.IsConstructedGenericType && @interface.GetGenericTypeDefinition() == interfaceTarget);

    return interfaces.Any(@interface => @interface == interfaceTarget);
  }

  public static Type GetType(string typeName) {
    Type typeResult = null;
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

    foreach (var assembly in assemblies)
    foreach (var type in assembly.GetTypes())
      if (type.Name.Equals(typeName)) {
        typeResult = type;
        break;
      }

    return typeResult;
  }

  public static IEnumerable<Type> GetConcreteTypesOf<T>() => GetTypesOf<T>();

  // return Assembly
  //       .GetAssembly(typeof(T))
  //       .GetTypes()
  //       .Where(p => typeof(T)
  //       .IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
  //       .ToList();
  public static IEnumerable<Type> GetTypesOf<T>(bool isClass = true, bool isAbstract = false) {
    if (isClass && !isAbstract)
      return Assembly
        .GetAssembly(typeof(T))
        .GetTypes()
        .Where(p => typeof(T).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

    // TODO: implement all isClass cases
    return Enumerable.Empty<Type>();
  }

  public static IEnumerable<string> GetConcreteTypeNamesOf<T>() {
    return GetConcreteTypesOf<T>().Select(type => type.Name);
  }

  public static IEnumerable<string> GetConcreteTypeQualifiedNamesOf<T>() {
    return GetConcreteTypesOf<T>().Select(type => type.AssemblyQualifiedName);
  }

  public static IEnumerable<T> CreateInstancesOf<T>() where T : class {
    return GetConcreteTypesOf<T>().Select(type => Activator.CreateInstance(type) as T);

    // var instances = new List<T>();
    // GetConcreteTypesOf<T>().ForEach(type => { instances.Add(Activator.CreateInstance(type) as T); });
    //
    // return instances;
  }

  public static MethodInfo GetNonPublicMethod(this Type type, string methodName, Type paramType) {
    return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder,
      new[] {paramType}, null);
  }

  /// <summary>
  ///   Check type of the first parameter.
  /// </summary>
  public static bool IsParamTypeOf<T>(this MethodInfo method) => method.GetParameters()[0].ParameterType == typeof(T);

  /// <summary>
  ///   Check method not null and type of the first parameter.
  /// </summary>
  public static bool ExistWithParamTypeOf<T>(this MethodInfo method) => method != null && method.IsParamTypeOf<T>();

  // public static bool HasOverloadForArgument(Type targetType, string methodName, object arg) {
  //   var methodInfo = targetType.GetMethod(name: methodName, types: new[] { arg.GetType() });
  //   return methodInfo != null;
  // }

  public static bool HasOverloadForArgument(this Type targetType, string methodName, object arg) {
    var methodInfo = targetType.GetMethod(methodName, new[] {arg.GetType()});
    return methodInfo != null;
  }
}
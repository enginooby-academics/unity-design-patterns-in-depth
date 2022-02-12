using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ? Rename to ReflectionUtils
public static class TypeUtils {
  // TIP: Use generics to pass type as argument with shorter syntax
  public static bool IsSubclassOf<T>(this Type type) => type.IsSubclassOf(typeof(T));

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
    if (interfaceTarget.IsGenericTypeDefinition) {
      foreach (var @interface in interfaces)
        if (@interface.IsConstructedGenericType && @interface.GetGenericTypeDefinition() == interfaceTarget)
          return true;
    }
    else {
      foreach (var @interface in interfaces)
        if (@interface == interfaceTarget)
          return true;
    }

    return false;
  }

  public static List<Type> GetConcreteTypesOf<T>() => GetTypesOf<T>();

  // return Assembly
  //       .GetAssembly(typeof(T))
  //       .GetTypes()
  //       .Where(p => typeof(T)
  //       .IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
  //       .ToList();
  public static List<Type> GetTypesOf<T>(bool isClass = true, bool isAbstract = false) {
    var types = new List<Type>();
    if (isClass && !isAbstract)
      types = Assembly
        .GetAssembly(typeof(T))
        .GetTypes()
        .Where(p => typeof(T)
          .IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
        .ToList();

    // TODO: implement all isClass cases
    return types;
  }

  public static List<string> GetConcreteTypeNamesOf<T>() {
    var types = GetConcreteTypesOf<T>();
    var _typeNames = new List<string>();

    for (var i = 0; i < types.Count; i++) _typeNames.Add(types[i].Name);
    return _typeNames;
  }

  public static List<string> GetConcreteTypeQualifiedNamesOf<T>() {
    var types = GetConcreteTypesOf<T>();
    var _typeNames = new List<string>();

    for (var i = 0; i < types.Count; i++) _typeNames.Add(types[i].AssemblyQualifiedName);
    return _typeNames;
  }

  public static List<T> GetInstancesOf<T>() where T : class {
    var instances = new List<T>();
    GetConcreteTypesOf<T>().ForEach(type => { instances.Add(Activator.CreateInstance(type) as T); });

    return instances;
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
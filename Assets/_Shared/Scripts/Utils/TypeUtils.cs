using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

public static class TypeUtils {
  public static List<Type> GetConcreteTypesOf<T>() {
    return GetTypesOf<T>(isClass: true, isAbstract: false);
    // return Assembly
    //       .GetAssembly(typeof(T))
    //       .GetTypes()
    //       .Where(p => typeof(T)
    //       .IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
    //       .ToList();
  }

  public static List<Type> GetTypesOf<T>(bool isClass = true, bool isAbstract = false) {
    var types = new List<Type>();
    if (isClass && !isAbstract) {
      types = Assembly
            .GetAssembly(typeof(T))
            .GetTypes()
            .Where(p => typeof(T)
            .IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
            .ToList();
    }

    // TODO: implement all isClass cases
    return types;
  }

  public static List<String> GetConcreteTypeNamesOf<T>() {
    var types = TypeUtils.GetConcreteTypesOf<T>();
    var _typeNames = new List<String>();

    for (int i = 0; i < types.Count; i++) {
      _typeNames.Add(types[i].Name);
    }
    return _typeNames;
  }

  public static List<T> GetInstancesOf<T>() where T : class {
    var instances = new List<T>();
    GetConcreteTypesOf<T>().ForEach(type => {
      instances.Add(Activator.CreateInstance(type) as T);
    });

    return instances;
  }


  // public static bool HasOverloadForArgument(Type targetType, string methodName, object arg) {
  //   var methodInfo = targetType.GetMethod(name: methodName, types: new[] { arg.GetType() });
  //   return methodInfo != null;
  // }

  public static bool HasOverloadForArgument(this Type targetType, string methodName, object arg) {
    var methodInfo = targetType.GetMethod(name: methodName, types: new[] { arg.GetType() });
    return methodInfo != null;
  }
}
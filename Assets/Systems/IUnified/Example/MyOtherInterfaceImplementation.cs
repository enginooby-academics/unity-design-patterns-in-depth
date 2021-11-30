using UnityEngine;

namespace Assets.IUnified.Example
{
    public class MyOtherInterfaceImplementation : MonoBehaviour, IMyInterface
    {
        public string StringProperty { get  { return $"{nameof(MyOtherInterfaceImplementation)}.{nameof(StringProperty)}"; } }

        public void Method()
        {
            Debug.Log($"{nameof(MyOtherInterfaceImplementation)}.{nameof(Method)}()");
        }
    }
}
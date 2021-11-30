using UnityEngine;

namespace Assets.IUnified.Example
{
    [CreateAssetMenu(fileName = "MyScriptableObjectImplementation")]
    public class MyScriptableObjectImplementation : ScriptableObject, IMyInterface
    {
        public string StringProperty { get { return $"{nameof(MyScriptableObjectImplementation)}.{nameof(StringProperty)}"; } }

        public void Method()
        {
            Debug.Log($"{nameof(MyScriptableObjectImplementation)}.{nameof(Method)}()");
        }
    }
}
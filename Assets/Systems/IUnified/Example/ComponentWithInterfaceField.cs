using UnityEngine;

namespace Assets.IUnified.Example
{
    public class ComponentWithInterfaceField : MonoBehaviour
    {
        public IMyInterface MyInterface
        {
            get { return _myInterfaceContainer.Result; }
            set { _myInterfaceContainer.Result = value; }
        }

        [SerializeField]
        private MyInterfaceContainer _myInterfaceContainer = null;

        private void OnValidate()
        {
            if(MyInterface != null)
            {
                Debug.Log(MyInterface.StringProperty);
                MyInterface.Method();
            }
        }
    }
}
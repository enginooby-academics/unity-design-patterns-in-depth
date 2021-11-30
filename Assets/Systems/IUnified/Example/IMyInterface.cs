using System;

namespace Assets.IUnified.Example
{
    public interface IMyInterface
    {
        string StringProperty { get; }

        void Method();
    }

    [Serializable]
    public class MyInterfaceContainer : IUnifiedContainer<IMyInterface> { }
}
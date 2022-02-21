using UnityEngine;

namespace AbstractDocumentPattern.Case2.Base1 {
  public class CarDriver : MonoBehaviour {
    private void Start() {
      // var carProperties = new Dictionary<Property, object>();
      // carProperties.Add(Property.Price, 10000f);
      // carProperties.Add(Property.Model, "300SL");
      // var car = new Car(carProperties);

      var car = new Car();
      car.Put(Property.Price, 10000f);
      // car.Put(Property.Model, "300SL");

#if UNITY_2021_2_OR_NEWER
      ((IHasPrice) car).GetPrice().Log();
      ((IHasModel) car).GetModel().Log();
#endif
    }
  }
}
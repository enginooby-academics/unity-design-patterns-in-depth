using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class SnapEditor : MonoBehaviour {
  [SerializeField] private TextMesh coordLabel;
  [SerializeField] private float gridSize = 10f;

  [Tooltip("Update object name match with grid coordinates")] [SerializeField]
  private bool coordNamed = true;

  private Vector3 snapPos;

  // Start is called before the first frame update
  private void Start() {
  }

  // Update is called once per frame
  private void Update() {
    if (!Application.isPlaying) SnapToGrid();

    if (coordLabel) UpdateLabel();
  }

  private void SnapToGrid() {
    snapPos.x = Mathf.RoundToInt(transform.position.x / gridSize) * gridSize;
    snapPos.z = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;
    transform.position = snapPos;
  }

  private void UpdateLabel() {
    coordLabel.text = Mathf.RoundToInt(transform.position.x / gridSize) + "," +
                      Mathf.RoundToInt(transform.position.z / gridSize);
    if (coordNamed) name = coordLabel.text;
  }
}
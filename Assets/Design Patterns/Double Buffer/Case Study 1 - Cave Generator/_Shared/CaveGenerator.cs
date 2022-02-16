using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleBufferPattern.Case1 {
  public class CaveGenerator : MonoBehaviour {
    //Is used to init the cellular automata by spreading random dots on a grid
    //And from these dots we will generate caves
    //The higher the fill percentage, the smaller the caves
    [Range(0, 1)] [SerializeField] protected float _fillPercent = .5f;

    protected const int CAVE_ID = 0;
    protected const int WALL_ID = 1;

    //The size of the grid
    protected const int GRID_SIZE = 100;

    //How many steps do we want to simulate?
    protected const int SIMULATION_STEPS = 20;

    //For how long will we pause between each simulation step so we can look at the result
    protected const float PAUSE_TIME = 1f;

    //Display the cave pattern on this plane 
    private MeshRenderer _renderer;

    private void Awake() {
      _renderer = GetComponent<MeshRenderer>();

      //To get the same random numbers each time we run the script
      Random.InitState(100);
    }

    //Init the old values so we can calculate the new values
    protected void InitBuffer(int[,] buffer) {
      //Init the old values so we can calculate the new values
      for (var x = 0; x < GRID_SIZE; x++)
      for (var y = 0; y < GRID_SIZE; y++)
        //We dont want holes in our walls, so the border is always a wall
        if (IsCellAtBorder(x, y))
          buffer[x, y] = WALL_ID;
        else
          //Random walls and caves
          buffer[x, y] = Random.Range(0f, 1f) < _fillPercent ? WALL_ID : CAVE_ID;

      //For testing that init is working
      // GenerateAndDisplayTexture(buffer);
    }

    protected bool IsCellAtBorder(int cellX, int cellY) =>
      cellX == 0 || cellX == GRID_SIZE - 1 || cellY == 0 || cellY == GRID_SIZE - 1;

    protected void OnCaveCompleted() => Debug.Log("Cave is completely generated.");

    //Given a cell, how many of the 8 surrounding cells are walls?
    protected int GetSurroundingWallCount(int cellX, int cellY, int[,] buffer) {
      var wallCount = 0;

      //We dont need to care about being outside of the grid because we are never looking at the border
      for (var neighborX = cellX - 1; neighborX <= cellX + 1; neighborX++)
      for (var neighborY = cellY - 1; neighborY <= cellY + 1; neighborY++) {
        //This is the cell itself and no neighbor!
        if (neighborX == cellX && neighborY == cellY) continue;
        if (buffer[neighborX, neighborY] == WALL_ID) wallCount++;
      }

      return wallCount;
    }

    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    protected void GenerateAndDisplayTexture(int[,] buffer) {
      //We are constantly creating new textures, so we have to delete old textures or the memory will keep increasing
      //The garbage collector is not collecting unused textures
      Resources.UnloadUnusedAssets();
      //We could also use 
      //Destroy(_renderer.sharedMaterial.mainTexture);
      //Or reuse the same texture

      //These two arrays are always the same so we could init them once at start
      var texture = new Texture2D(GRID_SIZE, GRID_SIZE) {
        filterMode = FilterMode.Point,
      };
      var textureColors = new Color[GRID_SIZE * GRID_SIZE];

      for (var y = 0; y < GRID_SIZE; y++)
      for (var x = 0; x < GRID_SIZE; x++)
        //From 2d array to 1d array
        textureColors[y * GRID_SIZE + x] = buffer[x, y] == WALL_ID ? Color.black : Color.white;

      texture.SetPixels(textureColors);
      texture.Apply();
      _renderer.sharedMaterial.mainTexture = texture;
    }
  }
}
using System.Collections;
using UnityEngine;

namespace DoubleBufferPattern.Case1.Naive {
  public class SingleBufferCaveGenerator : CaveGenerator {
    private int[,] _buffer;

    private void Start() {
      _buffer = new int[GRID_SIZE, GRID_SIZE];
      InitBuffer(_buffer);
      //Start the simulation
      StartCoroutine(SimulateCavePatternCoroutine());
    }

    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePatternCoroutine() {
      for (var i = 0; i < SIMULATION_STEPS; i++) {
        //Calculate the new values
        RunCellularAutomataStep();
        //Generate texture and display it on the plane
        GenerateAndDisplayTexture(_buffer);

        yield return new WaitForSeconds(PAUSE_TIME);
      }

      OnCaveCompleted();
    }

    //Generate caves by smoothing the data
    private void RunCellularAutomataStep() {
      for (var x = 0; x < GRID_SIZE; x++)
      for (var y = 0; y < GRID_SIZE; y++) {
        //Border is always wall
        if (IsCellAtBorder(x, y)) {
          _buffer[x, y] = WALL_ID;

          continue;
        }

        var surroundingWalls = GetSurroundingWallCount(x, y, _buffer);

        //Use some smoothing rules to generate caves
        if (surroundingWalls > 4)
          _buffer[x, y] = WALL_ID;
        else if (surroundingWalls < 4)
          _buffer[x, y] = CAVE_ID;
      }
    }
  }
}
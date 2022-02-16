using System.Collections;
using UnityEngine;

namespace DoubleBufferPattern.Case1.Base1 {
  //Illustrate the Double Buffer game programming pattern by generating a cave pattern and display it on a plane
  //The idea is based on this Unity tutorial https://www.youtube.com/watch?v=v7yyZZjF1z4 
  public class DoubleBufferCaveGenerator : CaveGenerator {
    //The double buffer
    //Notice that the Unity tutorial is using one buffer,
    //which works but is not entirely correct because it results in a strong diagonal bias
    //Someone in the comment section is also complaining about it, so this is the correct version
    //Is storing int where 1 is wall and 0 is cave
    private int[,] _buffer1, _buffer2;

    private void Start() {
      _buffer1 = _buffer2 = new int[GRID_SIZE, GRID_SIZE];
      InitBuffer(_buffer1);
      //Start the simulation
      StartCoroutine(SimulateCavePatternCoroutine());
    }

    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePatternCoroutine() {
      for (var i = 0; i < SIMULATION_STEPS; i++) {
        //Calculate the new values
        RunCellularAutomataStep();
        //Generate texture and display it on the plane
        GenerateAndDisplayTexture(_buffer2);
        //Swap the pointers to the buffers
        (_buffer1, _buffer2) = (_buffer2, _buffer1);

        yield return new WaitForSeconds(PAUSE_TIME);
      }

      OnCaveCompleted();
    }

    //Generate caves by smoothing the data
    //Remember to always put the new results in bufferNew and use bufferOld to do the calculations
    private void RunCellularAutomataStep() {
      for (var x = 0; x < GRID_SIZE; x++)
      for (var y = 0; y < GRID_SIZE; y++) {
        //Border is always wall
        if (IsCellAtBorder(x, y)) {
          _buffer2[x, y] = WALL_ID;

          continue;
        }

        //Uses bufferOld to get the wall count
        var surroundingWalls = GetSurroundingWallCount(x, y, _buffer1);

        //Use some smoothing rules to generate caves
        if (surroundingWalls > 4)
          _buffer2[x, y] = WALL_ID;
        else if (surroundingWalls == 4)
          _buffer2[x, y] = _buffer1[x, y];
        else
          _buffer2[x, y] = CAVE_ID;
      }
    }
  }
}
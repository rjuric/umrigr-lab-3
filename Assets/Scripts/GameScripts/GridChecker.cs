using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameScene
{
    public static class GridChecker
    {
        public static bool CheckRotationAvailabilityWithDisplacements(Transform tetrominoTransform, Tetromino tetrominoStats, TetrisGrid grid, Vector3 gridPosition)
        {
            Vector3 position = tetrominoTransform.position;
            Vector3 priv;

            if (CheckGrid(tetrominoTransform, grid, gridPosition))
            {
                return true;
            }

            priv = position;
            priv.x -= 1;
            tetrominoTransform.position = priv;

            if (CheckGrid(tetrominoTransform, grid, gridPosition))
            {
                return true;
            }

            priv = position;
            priv.x += 1;
            tetrominoTransform.position = priv;

            if (CheckGrid(tetrominoTransform, grid, gridPosition))
            {
                return true;
            }

            /*
             * Todo: 
             * Implementacija provjera dostupnosti dispozicija nakon pomaka za jedno mjesto 
             * prema gore i jedno mjesto prema dolje. Također ako je tetromino "long" potrebno 
             * je provjeriti dispozicije za dva mjesta u desno i dva mjesta u lijevo.
             * 
            */

            priv = position;
            priv.y += 1;
            tetrominoTransform.position = priv;
            if (CheckGrid(tetrominoTransform, grid, gridPosition)) {
                return true;
            }

            priv = position;
            priv.y -= 1;
            tetrominoTransform.position = priv;
            if (CheckGrid(tetrominoTransform, grid, gridPosition)) {
                return true;
            }

            bool isLong = String.Equals(tetrominoStats.tetName, "long");
            if (isLong) {
                priv = position;
                priv.x -= 2;
                tetrominoTransform.position = priv;
                if (CheckGrid(tetrominoTransform, grid, gridPosition)) {
                    return true;
                }

                priv = position;
                priv.x += 2;
                tetrominoTransform.position = priv;
                if (CheckGrid(tetrominoTransform, grid, gridPosition)) {
                    return true;
                }
            }
            
            tetrominoTransform.position = position;
            Debug.Log("Nije proslo");
            return false;           
        }

        private static bool CheckGrid(Transform tetrominoTransform, TetrisGrid grid, Vector3 position)
        {
            bool availablePosition = true;
            foreach (Transform childTile in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(childTile.position.x - position.x);
                int y = (int)Mathf.Floor(childTile.position.y - position.y);
                if (y >= grid.height)
                {
                    if (x >= grid.width || x < 0)
                    {
                        availablePosition = false;
                    }
                }
                else
                {
                    if (x >= 0 && x < grid.width && y >= 0)
                    {
                        if (grid.CheckIfFieldEmpty(y, x) == false)
                        {
                            availablePosition = false;
                        }
                    }
                    else
                    {
                        availablePosition = false;
                    }
                }
            }

            return availablePosition;
        }

        public static void CheckLinesMovedAbovePiece(Transform tetrominoTransform, TetrisGrid grid, Vector3 gridPosition)
        {

            foreach (Transform childTile in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(childTile.position.x - gridPosition.x);
                int y = (int)Mathf.Floor(childTile.position.y - gridPosition.y);
                if (grid.CheckIfFieldEmpty(y, x) == false)
                {
                    int minY = int.MinValue;

                    foreach (Transform childTile2 in tetrominoTransform)
                    {
                        x = (int)Mathf.Floor(childTile2.position.x - gridPosition.x);
                        y = (int)Mathf.Floor(childTile2.position.y - gridPosition.y);
                        int privY = (int)gridPosition.y + grid.GetMinAvailableHeight(grid.height * 2, x);
                        if (privY > minY)
                        {
                            minY = privY;
                        }

                    }

                    Vector2 position = tetrominoTransform.position;
                    position.y = minY + 1;
                    tetrominoTransform.position = position;

                    break;
                }
            }
        }

    }
}

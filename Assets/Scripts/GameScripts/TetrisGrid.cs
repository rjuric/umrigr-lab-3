using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class TetrisGrid : MonoBehaviour
    {
        [SerializeField] private GameObject garbageTile;
        [SerializeField] private Transform spawner;
        private Transform[,] matrix;

        public int height = 20;
        public int width = 10;

        private void Start()
        {
            matrix = new Transform[height * 2, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = null;
                }
            }
        }

        public void Add(Transform tetromino, int y, int x)
        {
            matrix[y,x] = tetromino;
        }

        public bool CheckIfFieldEmpty(int y, int x)
        {
            bool isFieldEmpty = matrix[y, x] == null;
            return isFieldEmpty;
        }

        public int GetMinAvailableHeight(int y, int x)
        {
            /*
            * Todo: 
            * Implementirati tra�enje najni�e dostune lokacije u mre�i 
            * do koje se tetromino smije spustiti.
            */

            for (int i = y; i >= 0; i--) {
                if (matrix[i,x] != null) {
                    return i+1;
                }
            }

            return 0;
        }

        public void CheckForCompleteLines()
        {
            int completeLines = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                bool detector = true;
                for (int j = 0; j < width; j++)
                {
                    if (matrix[i, j] == null)
                    {
                        detector = false;
                        break;
                    }
                }

                if (detector)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Destroy(matrix[i, j].gameObject);
                    }
                    for (int k = i; k < matrix.GetLength(0) - 1; k++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (matrix[k + 1, j] != null)
                            {
                                matrix[k + 1, j].Translate(Vector2.down);
                            }
                            matrix[k, j] = matrix[k + 1, j];
                        }
                    }
                    for (int j = 0; j < width; j++)
                    {
                        matrix[matrix.GetLength(0) - 1, j] = null;
                    }

                    i = i - 1;
                    completeLines++;
                }
            }

            GameManager.Instance.SendGarbageLines(GetComponentInParent<TetrominoController>().playerName, completeLines - 1);
            GameManager.Instance.SumLines(GetComponentInParent<TetrominoController>().playerName, completeLines);
        }

        public void CreateGarbage()
        {
            for (int k = matrix.GetLength(0) - 2; k >= 0; k--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (matrix[k, j] != null)
                    {
                        matrix[k, j].Translate(Vector2.up);
                    }
                    matrix[k + 1, j] = matrix[k, j];
                }
            }

            int random = Random.Range(0, width);
            for (int j = 0; j < width; j++)
            {
                if (j != random)
                {
                    Vector3 position = new Vector3(spawner.position.x - (width / 2) + j + 0.5f, spawner.position.y - height + 0.5f, -3);
                    GameObject created = Instantiate(garbageTile, position, Quaternion.identity, transform.parent);
                    matrix[0, j] = created.transform;
                }
                else
                {
                    matrix[0, j] = null;
                }
            }

        }
    }
}

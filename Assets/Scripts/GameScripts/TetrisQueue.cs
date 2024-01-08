using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class TetrisQueue : MonoBehaviour
    {
        [SerializeField] private Transform holdPosition;        
        [SerializeField] private GameObject[] tetrominoPrefabs;
        [SerializeField] private Transform spawner;
        [SerializeField] private TetrominoController controller;
        [SerializeField] private int queueLenght;
        private GameObject currentlyHeld;
        private List<GameObject> queuePieces;
        private bool holdFull;

        public void FillQueue()
        {
            holdFull = false;
            queuePieces = new List<GameObject>();
            for (int i = 0; i < queueLenght; i++)
            {
                int random = Random.Range(0, tetrominoPrefabs.Length);
                GameObject created = Instantiate(tetrominoPrefabs[random], new Vector3(transform.position.x, transform.position.y - (i * 3), -3), Quaternion.identity, spawner.transform.parent);
                queuePieces.Add(created.gameObject);
            }
        }

        public void NextTetromino()
        {
            controller.SetControllerTetromino(queuePieces[0]);
            queuePieces[0].transform.position = spawner.position;
            queuePieces[0].GetComponent<Tetromino>().UpdateGhost();
            queuePieces.RemoveAt(0);

            for (int i = 0; i < queueLenght - 1; i++)
            {
                queuePieces[i].transform.Translate(new Vector3(0, 3, 0)); //= new Vector3(pieces[i].transform.position.x, pieces[i].transform.position.y + 3, pieces[i].transform.position.z);
            }
            int random = Random.Range(0, tetrominoPrefabs.Length);
            GameObject created = Instantiate(tetrominoPrefabs[random], new Vector3(transform.position.x, transform.position.y - (3 * 3), -3), Quaternion.identity, spawner.transform.parent);
            queuePieces.Add(created.gameObject);
        }

        public void HoldTetromino()
        {
            GameObject priv = currentlyHeld;

            controller.tetromino.GetComponent<Tetromino>().DestroyGhost();
            CreateTetrominoClone();

            if (holdFull)
            {
                priv.transform.position = spawner.position;
                controller.SetControllerTetromino(priv);
                priv.GetComponent<Tetromino>().UpdateGhost();
            }
            else
            {
                this.NextTetromino();
                holdFull = true;
            }

        }

        private void CreateTetrominoClone()
        {
            foreach (Transform child in controller.tetromino.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (GameObject piece in tetrominoPrefabs)
            {
                if (string.Equals(piece.GetComponent<Tetromino>().tetName, controller.tetromino.GetComponent<Tetromino>().tetName))
                {
                    Destroy(controller.tetromino.gameObject);
                    currentlyHeld = Instantiate(piece, holdPosition.position, Quaternion.identity, spawner.transform.parent);
                    break;
                }
            }
        }
    }
}

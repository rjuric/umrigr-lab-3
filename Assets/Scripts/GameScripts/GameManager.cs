using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] Tetrominos;
        [SerializeField] private Transform spawner1;
        [SerializeField] private Transform spawner2;
        [SerializeField] private TetrominoController tetrominoController1;
        [SerializeField] private TetrominoController tetrominoController2;
        [SerializeField] private TetrisQueue queue1;
        [SerializeField] private TetrisQueue queue2;
        private IEnumerator dropCoroutine;

        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void OnEnable()
        {
            TetrominoController.GameEnd += EndGame;
        }
        void OnDisable()
        {
            TetrominoController.GameEnd -= EndGame;
        }

        public void GameStart()
        {
            //napuniti red čekanja za prvog i za drugog igrača
            fillPlayerQueues();

            //za svaki red čekanja postaviti sljedeći tetromino
            setNextTetrominosForQueues();

            //definirati korutinu za bacanje novog tetromina
            dropCoroutine = DropTime();

            //pokrenuti korutinu za bacanje novog tetromina
            StartCoroutine(dropCoroutine);
        }

        private void fillPlayerQueues() {
            queue1.FillQueue();
            queue2.FillQueue();
        }

        private void setNextTetrominosForQueues() {
            queue1.NextTetromino();
            queue2.NextTetromino();
        }

        private IEnumerator DropTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                tetrominoController1.DropOnTick();
                tetrominoController2.DropOnTick();
            }
        }

        internal void EndGame(string playerName)
        {
            StopCoroutine(dropCoroutine);
            tetrominoController1.enabled = false;
            tetrominoController2.enabled = false;
        }

        public void SendGarbageLines(string playerName, int lines)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                for (int i = 0; i < lines; i++) 
                {
                    tetrominoController2.grid.CreateGarbage();
                }

                tetrominoController2.CheckLinesMovedAbovePiece();

                tetrominoController2.tetromino.GetComponent<Tetromino>().UpdateGhost();
            }
            else
            {
                for (int i = 0; i < lines; i++)
                {
                    tetrominoController1.grid.CreateGarbage();
                }

                tetrominoController2.CheckLinesMovedAbovePiece();

                tetrominoController1.tetromino.GetComponent<Tetromino>().UpdateGhost();
            }
        }

        public void SumLines(string playerName, int linesCleared)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                UIManager.Instance.SumLinesClearedPlayer1(linesCleared);
            }
            else
            {
                UIManager.Instance.SumLinesClearedPlayer2(linesCleared);
            }
        }

        internal void SumPiece(string playerName)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                UIManager.Instance.PiecesPlaced1++;
            }
            else
            {
                UIManager.Instance.PiecesPlaced2++;
            }
        }
    }
}

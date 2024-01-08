using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class Tetromino : MonoBehaviour
    {
        [SerializeField] private GameObject ghostParent;
        [SerializeField] private GameObject ghostChildren;

        private GameObject ghostingTetromino;

        public int left;
        public int right;
        public int up;
        public int down;
        public string tetName;

        private void Update()
        {
            foreach (Transform childTile in transform)
            {
                if (childTile.transform.position.y - transform.parent.position.y >= 20)
                {
                    childTile.gameObject.SetActive(false);
                }
                else if (childTile.gameObject.activeSelf == false)
                {
                    childTile.gameObject.SetActive(true);
                }
            }

        }

        public void UpdateGhost()
        {
            if (ghostingTetromino == null)
            {
                ghostingTetromino = Instantiate(ghostParent, transform.position, Quaternion.identity, this.transform.parent);
                foreach (Transform childTile in transform)
                {
                    Instantiate(ghostChildren, childTile.position, Quaternion.identity, ghostingTetromino.transform);
                }
            }

            int minY = int.MaxValue;

            ghostingTetromino.transform.position = this.transform.position;
            for (int i = 0; i < ghostingTetromino.transform.childCount; i++)
            {
                ghostingTetromino.transform.GetChild(i).position = this.transform.GetChild(i).position;
            }

            foreach (Transform childTile in transform)
            {
                int x = (int)(Mathf.Floor(childTile.position.x) - this.transform.parent.position.x);
                int y = (int)(Mathf.Floor(childTile.position.y) - this.transform.parent.position.y);
                if (x < 0)
                {
                    x = 0;
                }
                if (x > 9)
                {
                    x = 9;
                }
                int privY = (int)Mathf.Floor(childTile.position.y) - GetComponentInParent<TetrominoController>().grid.GetMinAvailableHeight(y, x);
                if (privY < minY)
                {
                    minY = privY;
                }

            }

            ghostingTetromino.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - minY, -1f);

            foreach (Transform ghostChildTile in ghostingTetromino.transform)
            {
                if (ghostChildTile.transform.position.y - transform.parent.position.y >= 20)
                {
                    ghostChildTile.gameObject.SetActive(false);
                }
                else if (ghostChildTile.gameObject.activeSelf == false)
                {
                    ghostChildTile.gameObject.SetActive(true);
                }
            }
        }

        public void DestroyGhost()
        {
            foreach (Transform ghostChildTile in ghostingTetromino.transform)
            {
                Destroy(ghostChildTile.gameObject);
            }
            Destroy(ghostingTetromino.gameObject);
        }

        public void LockTetromino()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform childTile in transform)
            {
                if (childTile.transform.position.y - transform.parent.position.y >= 20)
                {
                    childTile.gameObject.SetActive(false);
                }
                else if (childTile.gameObject.activeSelf == false)
                {
                    childTile.gameObject.SetActive(true);
                }

                children.Add(childTile);
                int x = (int)Mathf.Floor(childTile.position.x - transform.parent.position.x);
                int y = (int)Mathf.Floor(childTile.position.y - transform.parent.position.y);

                GetComponentInParent<TetrominoController>().grid.Add(childTile.transform, y, x);
            }

            foreach (Transform childTile in children)
            {
                childTile.parent = this.transform.parent;
            }

            DestroyGhost();
            Destroy(gameObject);

            GetComponentInParent<TetrominoController>().grid.CheckForCompleteLines();
        }

    }
}

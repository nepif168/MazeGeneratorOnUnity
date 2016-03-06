/*
* 迷路生成
* 迷路のサイズは奇数にすること
* 大きすぎるとPCが悲鳴を上げる
*/

using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour {

    [SerializeField]
    Vector2 mazeSize;   //サイズ

    bool[,] mazeFlag;   //true:道 false:壁

	void Start () {
        CheckSettingError();
        GenerateMaze();
	}

    //設定でエラーがあれば表示
    void CheckSettingError()
    {
        if (mazeSize.x % 2 == 0 || mazeSize.y % 2 == 0) Debug.LogError("奇数にしてください");
        if (mazeSize.x < 5 || mazeSize.y < 5) Debug.LogError("迷路のサイズを大きくしてください");
    }

    void GenerateMaze()
    {
        mazeFlag = new bool[(int)mazeSize.x, (int)mazeSize.y];  //初期化

        //奇数の点をスタート位置に設定
        int x, y;
        do { x = Random.Range(0, (int)mazeSize.x); } while (x % 2 == 0);
        do { y = Random.Range(0, (int)mazeSize.y); } while (y % 2 == 0);

        //穴掘り法
        Recursion(x, y);
        //さっきの情報を元に生成
        CreateWall();
    }

    void CreateWall()
    {
        for (int y = 0; y < mazeSize.y; y++)
        {
            for (int x = 0; x < mazeSize.x; x++)
            {
                if (!mazeFlag[x, y])
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x, 0, y);
                }
            }
        }
    }

    void Recursion(int r, int c)
    {
        int[] randDirs = new int[] { 0, 1, 2, 3 };
        Shuffle(randDirs);  //シャッフル

        Debug.Log(randDirs[0]);

        for (int i = 0; i < randDirs.Length; i++)
        {
            switch (randDirs[i])
            {
                //←
                case 0:
                    if (r - 2 <= 0)
                        continue;
                    if (!mazeFlag[r - 2, c])
                    {
                        mazeFlag[r - 1, c] = true;
                        mazeFlag[r - 2, c] = true;
                        Recursion(r - 2, c);
                    }
                    break;

                //→
                case 1:
                    if (r + 2 >= mazeSize.x - 1)
                        continue;
                    if (!mazeFlag[r + 2, c])
                    {
                        mazeFlag[r + 1, c] = true;
                        mazeFlag[r + 2, c] = true;
                        Recursion(r + 2, c);
                    }
                    break;

                //↓
                case 2:
                    if (c - 2 <= 0)
                        continue;
                    if (!mazeFlag[r, c - 2])
                    {
                        mazeFlag[r, c - 1] = true;
                        mazeFlag[r, c - 2] = true;
                        Recursion(r, c - 2);
                    }
                    break;

                //↑
                case 3:

                    if (c + 2 >= mazeSize.y - 1)
                        continue;
                    if (!mazeFlag[r, c + 2])
                    {
                        mazeFlag[r, c + 1] = true;
                        mazeFlag[r, c + 2] = true;
                        Recursion(r, c + 2);
                    }
                    break;
            }
        }
    }

    //配列内をランダムに入れ替える
    void Shuffle(int[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            int temp = deck[i];
            int randomIndex = Random.Range(0, deck.Length);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] private int width = 30;
    [SerializeField] private int height = 20;
    [SerializeField] private int cellSize = 1;
    
    [Header("Visualization")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private Transform mazeParent;
    
    private Cell[,] maze;
    private System.Random random;

    // 셀의 벽 상태를 나타내는 구조체
    [System.Serializable]
    public class Cell
    {
        public bool northWall = true;
        public bool southWall = true;
        public bool eastWall = true;
        public bool westWall = true;
        public bool visited = false;
    }

    void Start()
    {
        random = new System.Random();
        Debug.Log("start maze generation");
        GenerateMaze();
        Debug.Log("maze generation complete");
    }
    
    /// <summary>
    /// 미로를 생성합니다
    /// </summary>
    public void GenerateMaze()
    {
        InitializeMaze();
        RecursiveBacktracking(0, 0);
    // maze[0, 0].westWall = false;
        maze[width - 1, height - 1].eastWall = false;

        if (mazeParent != null && wallPrefab != null && floorPrefab != null)
        {
            VisualizeMaze();
        }
    }
    
    /// <summary>
    /// 미로 배열을 초기화합니다 (모든 벽이 있는 상태)
    /// </summary>
    private void InitializeMaze()
    {
        maze = new Cell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new Cell();
            }
        }
    }
    
    /// <summary>
    /// Recursive Backtracking 알고리즘으로 미로를 생성합니다
    /// </summary>
    private void RecursiveBacktracking(int x, int y)
    {
        maze[x, y].visited = true;
        
        // 방향: 북, 남, 동, 서
        int[] directions = { 0, 1, 2, 3 };
        
        // 방향을 랜덤하게 섞기
        ShuffleArray(directions);
        
        foreach (int dir in directions)
        {
            int newX = x;
            int newY = y;
            
            switch (dir)
            {
                case 0: // 북
                    newY = y + 1;
                    break;
                case 1: // 남
                    newY = y - 1;
                    break;
                case 2: // 동
                    newX = x + 1;
                    break;
                case 3: // 서
                    newX = x - 1;
                    break;
            }
            
            // 경계 체크 및 방문 여부 확인
            if (IsValidCell(newX, newY) && !maze[newX, newY].visited)
            {
                // 벽 제거
                RemoveWall(x, y, dir);
                RemoveWall(newX, newY, GetOppositeDirection(dir));
                
                // 재귀 호출
                RecursiveBacktracking(newX, newY);
            }
        }
    }
    
    /// <summary>
    /// 배열을 랜덤하게 섞습니다 (Fisher-Yates 알고리즘)
    /// </summary>
    private void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    
    /// <summary>
    /// 셀이 유효한 범위 내에 있는지 확인합니다
    /// </summary>
    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
    /// <summary>
    /// 벽을 제거합니다
    /// </summary>
    private void RemoveWall(int x, int y, int direction)
    {
        Cell cell = maze[x, y];
        
        switch (direction)
        {
            case 0: // 북
                cell.northWall = false;
                break;
            case 1: // 남
                cell.southWall = false;
                break;
            case 2: // 동
                cell.eastWall = false;
                break;
            case 3: // 서
                cell.westWall = false;
                break;
        }
        
        maze[x, y] = cell;
    }
    
    /// <summary>
    /// 반대 방향을 반환합니다
    /// </summary>
    private int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case 0: return 1; // 북 -> 남
            case 1: return 0; // 남 -> 북
            case 2: return 3; // 동 -> 서
            case 3: return 2; // 서 -> 동
            default: return -1;
        }
    }
    
    /// <summary>
    /// 생성된 미로를 시각화합니다
    /// </summary>
    private void VisualizeMaze()
    {
        // 기존 미로 삭제
        if (mazeParent != null)
        {
            foreach (Transform child in mazeParent)
            {
                Destroy(child.gameObject);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);

                // 바닥 생성
                if (floorPrefab != null)
                {
                    GameObject floor = Instantiate(floorPrefab, cellPosition, Quaternion.identity, mazeParent);
                    floor.transform.localScale = Vector3.one * cellSize;
                }

                // 벽 생성
                if (wallPrefab != null)
                {
                    Vector3 wallOffset = Vector3.zero;

                    // 북쪽 벽
                    if (maze[x, y].northWall)
                    {
                        wallOffset = new Vector3(0, 0, cellSize * 0.5f);
                        CreateWall(cellPosition + wallOffset, Quaternion.identity);
                    }

                    // 남쪽 벽
                    if (maze[x, y].southWall)
                    {
                        wallOffset = new Vector3(0, 0, -cellSize * 0.5f);
                        CreateWall(cellPosition + wallOffset, Quaternion.identity);
                    }

                    // 동쪽 벽
                    if (maze[x, y].eastWall)
                    {
                        wallOffset = new Vector3(cellSize * 0.5f, 0, 0);
                        CreateWall(cellPosition + wallOffset, Quaternion.Euler(0, 90, 0));
                    }

                    // 서쪽 벽
                    if (maze[x, y].westWall)
                    {
                        wallOffset = new Vector3(-cellSize * 0.5f, 0, 0);
                        CreateWall(cellPosition + wallOffset, Quaternion.Euler(0, 90, 0));
                    }
                }
            }
        }
        Debug.Log(maze);
    }
    
    /// <summary>
    /// 벽 오브젝트를 생성합니다
    /// </summary>
    private void CreateWall(Vector3 position, Quaternion rotation)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation, mazeParent);
        wall.transform.localScale = new Vector3(cellSize * 1, 3, cellSize *  0.1f);
    }
    
    /// <summary>
    /// 미로 데이터를 가져옵니다 (다른 스크립트에서 사용 가능)
    /// </summary>
    public Cell[,] GetMaze()
    {
        return maze;
    }
    
    /// <summary>
    /// 특정 셀의 정보를 가져옵니다
    /// </summary>
    public Cell GetCell(int x, int y)
    {
        if (IsValidCell(x, y))
        {
            return maze[x, y];
        }
        return new Cell(); // 기본값 반환
    }
    
    /// <summary>
    /// 미로 크기를 설정합니다
    /// </summary>
    public void SetMazeSize(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;
    }
    
    /// <summary>
    /// 시드값을 설정하여 재현 가능한 미로를 생성합니다
    /// </summary>
    public void SetSeed(int seed)
    {

        random = new System.Random(seed);
    }
}


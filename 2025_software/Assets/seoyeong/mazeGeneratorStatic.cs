using System;
using System.Collections.Generic;

/// <summary>
/// 정적 클래스로 구현된 미로 생성기 (MonoBehaviour 없이 사용 가능)
/// Unity 스크립트에서 직접 호출할 수 있습니다
/// </summary>
public static class MazeGeneratorStatic
{
    /// <summary>
    /// 셀의 벽 상태를 나타내는 구조체
    /// </summary>
    [System.Serializable]
    public struct Cell
    {
        public bool northWall;
        public bool southWall;
        public bool eastWall;
        public bool westWall;
        public bool visited;
        
        public Cell(bool north, bool south, bool east, bool west)
        {
            northWall = north;
            southWall = south;
            eastWall = east;
            westWall = west;
            visited = false;
        }
    }
    
    /// <summary>
    /// 미로를 생성합니다
    /// </summary>
    /// <param name="width">미로의 너비</param>
    /// <param name="height">미로의 높이</param>
    /// <param name="seed">랜덤 시드 (선택사항)</param>
    /// <returns>생성된 미로 배열</returns>
    public static Cell[,] GenerateMaze(int width, int height, int? seed = null)
    {
        System.Random random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        Cell[,] maze = new Cell[width, height];
        
        // 미로 초기화 (모든 벽이 있는 상태)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new Cell(true, true, true, true);
            }
        }
        
        // Recursive Backtracking 알고리즘 실행
        RecursiveBacktracking(maze, 0, 0, random, width, height);
        
        return maze;
    }
    
    /// <summary>
    /// Recursive Backtracking 알고리즘으로 미로를 생성합니다
    /// </summary>
    private static void RecursiveBacktracking(Cell[,] maze, int x, int y, System.Random random, int width, int height)
    {
        Cell cell = maze[x, y];
        cell.visited = true;
        maze[x, y] = cell;
        
        // 방향: 북, 남, 동, 서
        int[] directions = { 0, 1, 2, 3 };
        
        // 방향을 랜덤하게 섞기
        ShuffleArray(directions, random);
        
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
            if (IsValidCell(newX, newY, width, height) && !maze[newX, newY].visited)
            {
                // 벽 제거
                RemoveWall(maze, x, y, dir);
                RemoveWall(maze, newX, newY, GetOppositeDirection(dir));
                
                // 재귀 호출
                RecursiveBacktracking(maze, newX, newY, random, width, height);
            }
        }
    }
    
    /// <summary>
    /// 배열을 랜덤하게 섞습니다 (Fisher-Yates 알고리즘)
    /// </summary>
    private static void ShuffleArray(int[] array, System.Random random)
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
    private static bool IsValidCell(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
    /// <summary>
    /// 벽을 제거합니다
    /// </summary>
    private static void RemoveWall(Cell[,] maze, int x, int y, int direction)
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
    private static int GetOppositeDirection(int direction)
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
    /// 특정 셀의 정보를 가져옵니다
    /// </summary>
    public static Cell GetCell(Cell[,] maze, int x, int y, int width, int height)
    {
        if (IsValidCell(x, y, width, height))
        {
            return maze[x, y];
        }
        return new Cell(true, true, true, true); // 기본값 반환
    }
    
    /// <summary>
    /// 두 셀 사이에 경로가 있는지 확인합니다
    /// </summary>
    public static bool HasPath(Cell cell, int direction)
    {
        switch (direction)
        {
            case 0: return !cell.northWall; // 북
            case 1: return !cell.southWall; // 남
            case 2: return !cell.eastWall;  // 동
            case 3: return !cell.westWall;  // 서
            default: return false;
        }
    }
}


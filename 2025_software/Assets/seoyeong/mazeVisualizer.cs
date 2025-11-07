using UnityEngine;

/// <summary>
/// 미로를 시각화하는 컴포넌트
/// MazeGeneratorStatic과 함께 사용할 수 있습니다
/// </summary>
public class MazeVisualizer : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private int cellSize = 1;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private Transform mazeParent;
    
    [Header("Generation")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private int seed = -1; // -1이면 랜덤
    
    private MazeGeneratorStatic.Cell[,] maze;
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateAndVisualize();
        }
    }
    
    /// <summary>
    /// 미로를 생성하고 시각화합니다
    /// </summary>
    public void GenerateAndVisualize()
    {
        // 미로 생성
        if (seed >= 0)
        {
            maze = MazeGeneratorStatic.GenerateMaze(width, height, seed);
        }
        else
        {
            maze = MazeGeneratorStatic.GenerateMaze(width, height);
        }
        
        // 시각화
        VisualizeMaze();
    }
    
    /// <summary>
    /// 미로를 시각화합니다
    /// </summary>
    private void VisualizeMaze()
    {
        // 기존 미로 삭제
        if (mazeParent != null)
        {
            foreach (Transform child in mazeParent)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
        else
        {
            mazeParent = transform;
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
                    floor.name = $"Floor_{x}_{y}";
                }
                
                // 벽 생성
                if (wallPrefab != null)
                {
                    MazeGeneratorStatic.Cell cell = maze[x, y];
                    
                    // 북쪽 벽
                    if (cell.northWall)
                    {
                        Vector3 wallOffset = new Vector3(0, 0, cellSize * 0.5f);
                        CreateWall(cellPosition + wallOffset, Quaternion.identity, $"Wall_N_{x}_{y}");
                    }
                    
                    // 남쪽 벽
                    if (cell.southWall)
                    {
                        Vector3 wallOffset = new Vector3(0, 0, -cellSize * 0.5f);
                        CreateWall(cellPosition + wallOffset, Quaternion.identity, $"Wall_S_{x}_{y}");
                    }
                    
                    // 동쪽 벽
                    if (cell.eastWall)
                    {
                        Vector3 wallOffset = new Vector3(cellSize * 0.5f, 0, 0);
                        CreateWall(cellPosition + wallOffset, Quaternion.Euler(0, 90, 0), $"Wall_E_{x}_{y}");
                    }
                    
                    // 서쪽 벽
                    if (cell.westWall)
                    {
                        Vector3 wallOffset = new Vector3(-cellSize * 0.5f, 0, 0);
                        CreateWall(cellPosition + wallOffset, Quaternion.Euler(0, 90, 0), $"Wall_W_{x}_{y}");
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 벽 오브젝트를 생성합니다
    /// </summary>
    private void CreateWall(Vector3 position, Quaternion rotation, string name)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation, mazeParent);
        wall.transform.localScale = new Vector3(cellSize * 0.1f, 1, cellSize * 0.1f);
        wall.name = name;
    }
    
    /// <summary>
    /// 미로 데이터를 가져옵니다
    /// </summary>
    public MazeGeneratorStatic.Cell[,] GetMaze()
    {
        return maze;
    }
    
    /// <summary>
    /// 특정 셀의 정보를 가져옵니다
    /// </summary>
    public MazeGeneratorStatic.Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return maze[x, y];
        }
        return new MazeGeneratorStatic.Cell(true, true, true, true);
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
    /// 시드값을 설정합니다
    /// </summary>
    public void SetSeed(int newSeed)
    {
        seed = newSeed;
    }
}


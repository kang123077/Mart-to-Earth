using UnityEngine;
/// <summary>
/// static GameInfo나 진배없음
/// </summary>
public static class MapInfo
{
    // 현재 맵의 시드 넘버
    public static int seed_Number = 0;
    // 난이도, 시간 * a + 스테이지 * a 의 비율로 증가
    public static float difficulty = 0;
    // cur_Stage는 MapManager의 초기화 이후에 1로 증가
    public static int cur_Stage = 0;
    // 현재 방의 갯수
    public static int node_num = 4;
    // 맵 생성 시 사용 될 노드 풀
    public static NodePool cur_NodePool = NodePool.All;
    // retry 확인 용
    public static bool isRetry = false;
    // 노드 생성 시 시작 방과의 거리에 따른 방 생성 확률
    public static float maxDistance = 3f;
    // 게임 시작 후 경과한 시간
    public static float cur_Time = 0;
    // 지금까지 먹은 아이템 갯수
    public static int core = 0;
    public static int hpCore = 0;
    public static int dmgCore = 0;
    public static int speedCore = 0;
    // 유저가 지금까지 부활한 적 있는지
    public static bool isRevive = false;
    // 유저가 지금까지 먹은 스토리 아이템 수
    public static int storyValue = GetUserStoryValue();

    // Pause인지 아닌지 나타내는 bool
    private static bool _gamePause = false;
    public static bool gamePause
    {
        get => _gamePause;
        set
        {
            _gamePause = value;
            if (value)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    // Pause를 요청하는 이벤트 갯수, 하나라도 있으면 pause = true
    private static int _pauseRequest = 0;
    public static int pauseRequest
    {
        get => _pauseRequest;
        set
        {
            _pauseRequest = value;
            if (value == 0)
            {
                gamePause = false;
            }
            else
            {
                gamePause = true;
            }
        }
    }

    public static void ResetValues()
    {
        seed_Number = 0;
        difficulty = 0;
        cur_Stage = 0;
        node_num = 4;
        cur_NodePool = NodePool.All;
        isRetry = false;
        maxDistance = 3f;
        cur_Time = 0;
        core = 0;
        hpCore = 0;
        dmgCore = 0;
        speedCore = 0;
    }

    public static int GetUserStoryValue()
    {
        if (PlayerPrefs.HasKey("storyValue"))
            return PlayerPrefs.GetInt("storyValue");
        else
            return 0;
    }
}

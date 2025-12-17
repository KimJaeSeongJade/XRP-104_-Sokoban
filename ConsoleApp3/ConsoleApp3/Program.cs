// 소코반 구현 구조
// 1. 초기 세팅
// 2. 화면에 출력
// 3. 사용자 입력
// 4. 로직 수행 (이동, 폭탄 밀기)
// 5. 2~4 반복

using System;
using System.Runtime.ExceptionServices;

class Program
{
    private const char PLAYER = 'P';        // 플레이어,
    private const char PLAYER_ON_GOAL = '@';// 플레이어가 목표지점 위에 있는 상태,
    private const char BOMB = 'B';          // 폭탄,
    private const char BOMB_ON_GOAL = '!';  // 폭탄이 목표지점 위에 있는 상태,
    private const char GOAL = 'G';          // 목표지점,
    private const char WALL = '#';          // 벽,
    private const char EMPTY = ' ';         // 빈공간

    private static char[,] map = new char[,] // 게임 필드(2차원 배열)
    {
        { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', 'G', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', 'G', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' }
    };
    
    static Position _playerPos = new Position()
    {
        X = 4, 
        Y = 4
    };

    private static int _moveCount = 0;
    
    static void Main(string[] args)
    {
        // 안내 멘트 출력
        PrintGuideText();
        
        while (true)
        {
            // 출력
            PrintMoveCount();
            PrintMap();
            
            // 클리어 시 반복 종료
            if(IsGameClear())
            {
                PrintClearText();
                break;
            }

            // 사용자 입력
            ConsoleKey inputKey;
            if (!TryGetInput(out inputKey)) continue;
            
            // 종료 처리
            if (inputKey == ConsoleKey.Q)
            {
                Console.WriteLine("\n게임을 종료합니다");
                break;
            }

            // 로직 수행 
            
            // 이동 가능한지 판단.
            Position nextPos = GetNextPosition(inputKey);
            
            if(IsOutOfArray(nextPos)) continue;
            
            char targetTile = GetTile(nextPos);
            if (targetTile == WALL) continue;

            // 이동 구현 (이동, 폭탄 밀기)
            
            // 플레이어 단순 이동 (Goal 위로 이동하는 것도 포함)
            if(targetTile == EMPTY || targetTile == GOAL)
            {
                Move(_playerPos, nextPos, PLAYER);
                _playerPos = nextPos;
                _moveCount++;
            }
            // 폭탄을 밀면서 이동
            else if (targetTile == BOMB || targetTile == BOMB_ON_GOAL)
            {
                if (TryPushBomb(nextPos))
                {
                    _playerPos = nextPos;
                    _moveCount++;
                }
            }

        }

        Console.WriteLine("게임 끝");
    }
    
    

    static void PrintGuideText()
    {
        Console.Clear();
        Console.WriteLine("W : 위로 / S : 아래로 / A : 왼쪽 / D : 오른쪽 / Q : 종료");
        Console.WriteLine("모든 폭탄을 목표지점으로 옮기세요");
        Console.WriteLine();
    }

    static void PrintMoveCount()
    {
        Console.SetCursorPosition(0,4);
        Console.WriteLine($"이동 거리  : {_moveCount}");
        Console.WriteLine();
    }

    static bool IsGameClear()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == BOMB || map[y, x] == GOAL)
                {
                    return false;
                }
            }
        }

        return true;
    }

    static void PrintClearText()
    {
        Console.WriteLine();
        Console.WriteLine("축하합니다. 클리어 하셨습니다");
        Console.WriteLine($"총 이동 거리 : {_moveCount}");
        Console.WriteLine();
    }

    /// <summary>
    /// 사용자 입력을 받고, 게임에서 사용하는 키인지 판단
    /// </summary>
    /// <param name="inputKey">입력된 키를 보관할 변수</param>
    /// <returns>게임에서 사용하는 키라면 true, 아니라면 false</returns>
    static bool TryGetInput(out ConsoleKey inputKey)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        inputKey = keyInfo.Key;
        
        return inputKey == ConsoleKey.W ||
               inputKey == ConsoleKey.A ||
               inputKey == ConsoleKey.S ||
               inputKey == ConsoleKey.D ||
               inputKey == ConsoleKey.Q;
    }

    /// <summary>
    /// 입력받은 키에 따라 다음 좌표정보를 반환하는 함수
    /// </summary>
    /// <param name="inputKey">입력 키</param>
    /// <returns>입력 키에 따른 다음 좌표 구조체</returns>
    static Position GetNextPosition(ConsoleKey inputKey)
    {
        int newX = _playerPos.X;
        int newY = _playerPos.Y;

        if (inputKey == ConsoleKey.W) newY--;
        else if (inputKey == ConsoleKey.S) newY++;
        else if (inputKey == ConsoleKey.A) newX--;
        else if (inputKey == ConsoleKey.D) newX++;

        return new Position()
        {
            X = newX,
            Y = newY
        };
    }

    /// <summary>
    /// 입력받은 좌표의 타일을 반환하는 함수
    /// </summary>
    /// <param name="pos">입력 위치</param>
    /// <returns>배열의 char 문자 반환</returns>
    static char GetTile(Position pos)
    {
        return map[pos.Y, pos.X];
    }

    /// <summary>
    /// 입력받은 좌표의 타일을 바꾸는 함수
    /// </summary>
    /// <param name="pos">바꿀 위치</param>
    /// <param name="tile">대상 타일</param>
    static void SetTile(Position pos, char tile)
    {
        map[pos.Y, pos.X] = tile;
    }

    /// <summary>
    /// 입력받은 위치가 배열의 범위 바깥인지 판단하는 함수
    /// </summary>
    /// <param name="pos">입력 위치</param>
    /// <returns>배열 인덱스 범위 밖이라면 true, 아니라면 false</returns>
    static bool IsOutOfArray(Position pos)
    {
        bool outX = pos.X < 0 || map.GetLength(1) <= pos.X;
        bool outY = pos.Y < 0 || map.GetLength(0) <= pos.Y;
        
        return outX || outY;
    }

    // 플레이어 이동으로 먼저 구현, 이후 박스 이동에도 재사용 예정
    static void Move(Position from, Position to, char target)
    {
        // 출발지점을 기존 타일로 바꾸기 
        char originTile = GetOriginTile(GetTile(from));
        SetTile(from, originTile);
        // 다음 위치에 표시될 심볼을 결정 후 세팅
        char targetTile = GetTile(to);
        char nextTile = GetConvertTile(target, targetTile);
        SetTile(to, nextTile);
         
    }

    static char GetConvertTile(char mover, char under)
    {
        // 플레이어라는 조건으로 작성.
        if (mover == PLAYER)
        {
            if (under == GOAL)
            {
                return PLAYER_ON_GOAL;
            }
            else
            {
                return PLAYER;
            }
        }
        else if (mover == BOMB)
        {
            if (under == GOAL)
            {
                return BOMB_ON_GOAL;
            }
            else
            {
                return BOMB;
            }
        }

        return under;
    }

    /// <summary>
    /// 겹쳐져 있던 원본 타일을 반환하는 함수
    /// </summary>
    /// <param name="tile">변환할 타일</param>
    /// <returns>변환된 타일</returns>
    static char GetOriginTile(char tile)
    {
        return tile switch
        {
            PLAYER => EMPTY,
            PLAYER_ON_GOAL => GOAL,
            BOMB => EMPTY,
            BOMB_ON_GOAL => GOAL,
            _ => tile
        };
    }

    /// <summary>
    /// 박스를 미는 함수
    /// </summary>
    /// <param name="bombPos">폭탄의 위치</param>
    /// <returns>폭탄을 성공적으로 밀었다면 true, 아니라면 false</returns>
    static bool TryPushBomb(Position bombPos)
    {
        // 방향
        Position direction = GetDirection(_playerPos, bombPos);
        // 구해진 방향으로 한 칸 전진했을 때의 위치
        Position nextPos = AddDirection(bombPos, direction);
        
        // 맵 밖으로 나가는지 확인
        if (IsOutOfArray(nextPos)) return false;
        // 밀릴 수 없는 경우
        char nextTile = GetTile(nextPos);
        if (!(nextTile == EMPTY || nextTile == GOAL)) return false;
        
        // 폭탄 이동
        Move(bombPos, nextPos, BOMB);
        
        // 플레이어를 폭탄이 있던 위치로 이동
        Move(_playerPos, bombPos, PLAYER);

        return true;
    }

    static Position GetDirection(Position form, Position to)
    {
        return new Position()
        {
            X = to.X - form.X,
            Y = to.Y - form.Y
        };
    }

    static Position AddDirection(Position pos, Position direction)
    {
        return new Position()
        {
            X = pos.X + direction.X,
            Y = pos.Y + direction.Y
        };
    }

    /// <summary>
    /// 맵 출력 함수
    /// </summary>
    static void PrintMap()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }
            Console.WriteLine();
        }
    }
}

public struct Position
{
    public int X;
    public int Y;
}
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
            
            
            // 로직 수행 (이동, 폭탄 밀기)
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
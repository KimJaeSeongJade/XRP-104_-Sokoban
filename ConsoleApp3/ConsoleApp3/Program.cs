// 소코반 구현 구조
// 1. 초기 세팅
// 2. 화면에 출력
// 3. 사용자 입력
// 4. 로직 수행 (이동, 폭탄 밀기)
// 5. 2~4 반복

using System;
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
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', 'P', 'G', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', 'G', ' ', ' ', '#' },
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
            
            // 사용자 입력
            // 로직 수행 (이동, 폭탄 밀기)
        }
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
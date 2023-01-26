using System.Text;

namespace ConsoleProject
{

    class Program
    {
        static void Main()                                                                                        
        {
            // a와 b 중 최댓값을 구한다.
            int Max(int a, int b)
            {
                int result = 0;

                if (a > b)
                {
                    result = a;
                }

                else if (a < b)
                {
                    result = b;
                }

                return result;
            }

            int Min(int a, int b)
            {
                int result = 0;

                result = (a < b) ? a : b;

                return result;
            }

            // 초기 세팅
            Console.ResetColor(); // 컬러를 초기화한다.
            Console.CursorVisible = false; // 커서를 숨긴다.
            Console.Title = "소코반게임"; // 타이틀을 설정한다.
            Console.BackgroundColor = ConsoleColor.Yellow; // 배경색을 설정한다.
            Console.ForegroundColor = ConsoleColor.DarkMagenta; // 글꼴색을 설정한다.
            Console.Clear(); // 출력된 내용을 지워준다.

            // 기호 상수 정의
            const int GOAL_COUNT = 2;
            const int BOX_COUNT = GOAL_COUNT;
            const int WALL_COUNT = 12;
            const int BAD_COUNT = 4;

            // 플레이어 위치를 저장하기 위한 변수
            int playerX = 1;
            int playerY = 1;

            // 플레이어의 이동 방향을 저장하기 위한 변수
            Direction playerMoveDirection = Direction.None;

            // 박스의 위치를 저장하기 위한 변수
            int[] boxpositionsX = { 26, 40 };
            int[] boxpositionsY = { 10, 20 };

            // 벽의 위치를 저장하기 위한 변수
            int[] wallpositonsX = { 17, 38, 9, 22, 15, 27, 10, 31, 20, 23, 39, 5 };
            int[] wallpositionsY = { 12, 9, 6, 14, 21, 2, 26, 19, 11, 25, 23, 16 };

            // 골인의 위치를 저장하기 위한 변수
            int[] goalpositionsX = { 19, 27 };
            int[] goalpositionsY = { 13, 22 };

            // 함정의 위치를 저장하기 위한 변수
            int[] baditempositionsX = { 27, 7, 30, 35 };
            int[] baditempositionsY = { 16, 21, 9, 20 };

            // 밀고 있는 박스를 저장하기 위한 변수
            int pushedBoxId = 0;

            // 게임 루프 구성
            while (true)
            {                
                // ------------------------------Render-----------------------------------
                // 이전 프레임을 지운다.
                Console.Clear();
                
                // 게임 배경을 출력한다.
                Console.WriteLine(" ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )           ▷ 소코반게임 게임방법");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              J : 플레이어");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              * : 밀어서 옮길 수 있는 작은 별");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              o : 골인 지점");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              | : 움직이지 않는 장애물");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              ? : 함정");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )              →, ←, ↑, ↓ : 조작키");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )           ▷ *을 o에 옮겨주시면 클리어입니다!!");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine("(                                                   )");
                Console.WriteLine(" ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");

                // 플레이어를 그린다.
                Console.ForegroundColor = ConsoleColor.DarkRed;
                RenderObject(playerX, playerY, "J");

                // 벽을 그린다.
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                for (int i = 0; i < WALL_COUNT; ++i)
                {
                    int wallX = wallpositonsX[i];
                    int wallY = wallpositionsY[i];

                    RenderObject(wallX, wallY, "I");
                }

                // 골을 그린다.
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                for (int i = 0; i < GOAL_COUNT; ++i)
                {
                    int goalX = goalpositionsX[i];
                    int goalY = goalpositionsY[i];

                    RenderObject(goalX, goalY, "o");
                }

                // 박스를 그린다.
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                for (int i = 0; i < BOX_COUNT; ++i)
                {
                    int boxX = boxpositionsX[i];
                    int boxY = boxpositionsY[i];

                    RenderObject(boxX, boxY, "*");

                    for (int j = 0; j < GOAL_COUNT; ++j)
                    {
                        int goalX = goalpositionsX[j];
                        int goalY = goalpositionsY[j];

                        if (boxX == goalX && boxY == goalY)
                        {
                            RenderObject(boxX, boxY, "★");
                        }
                    }
                }

                // 함정을 그린다.
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                for (int i = 0; i < BAD_COUNT; ++i)
                {
                    int badItemX = baditempositionsX[i];
                    int badItemY = baditempositionsY[i];

                    RenderObject(badItemX, badItemY, "?");                  
                }

                // 다시 처음 설정한 폰트색으로 돌아온다.
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                // ------------------------------ProcessInput-----------------------------
                ConsoleKey key = Console.ReadKey().Key; 
                // ------------------------------Update-----------------------------------

                // 플레이어 이동 처리
                if (key == ConsoleKey.LeftArrow)
                {
                    playerX = Max(1, playerX - 1);
                    playerMoveDirection = Direction.Left;
                }

                if (key == ConsoleKey.RightArrow)
                {
                    playerX = Min(50, playerX + 1);
                    playerMoveDirection = Direction.Right;
                }

                if (key == ConsoleKey.DownArrow)
                {
                    playerY = Min(27, playerY + 1);
                    playerMoveDirection = Direction.Down;
                }

                if (key == ConsoleKey.UpArrow)
                {
                    playerY = Max(1, playerY - 1);
                    playerMoveDirection = Direction.Up;
                }

                // 플레이어와 벽의 충돌 처리
                for (int WallId = 0; WallId < WALL_COUNT; ++WallId)
                {
                    int wallX = wallpositonsX[WallId];
                    int wallY = wallpositionsY[WallId];

                    if (playerX == wallX && playerY == wallY)
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                playerX = wallX + 1;
                                break;

                            case Direction.Right:
                                playerX = wallX - 1;
                                break;

                            case Direction.Up:
                                playerY = wallY + 1;
                                break;

                            case Direction.Down:
                                playerY = wallY - 1;
                                break;
                        }
                    }
                }

                // 박스 이동 처리
                // 플레이어가 박스를 밀었을 때라는 게 무엇을 의미하는가? => 플레이어가 이동했을 때 플레이어의 위치와 박스 위치가 겹쳤다.
                for (int BoxId = 0; BoxId < BOX_COUNT; ++BoxId)
                {
                    int boxX = boxpositionsX[BoxId];
                    int boxY = boxpositionsY[BoxId];

                    if (playerX == boxX && playerY == boxY)
                    {
                        // 박스를 밀면 된다. => 박스의 좌표를 바꾼다.
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                boxX = Math.Max(1, boxX - 1);
                                playerX = boxX + 1;
                                break;

                            case Direction.Right:
                                boxX = Math.Min(boxX + 1, 45);
                                playerX = boxX - 1;
                                break;

                            case Direction.Up:
                                boxY = Math.Max(boxY - 1, 1);
                                playerY = boxY + 1;
                                break;

                            case Direction.Down:
                                boxY = Math.Min(boxY + 1, 26);
                                playerY = boxY - 1;
                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이어의 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

                                return;
                        }

                        boxpositionsX[BoxId] = boxX;
                        boxpositionsY[BoxId] = boxY;

                        pushedBoxId = BoxId;
                    }
                }

                // 벽과 박스가 부딪혔을 때
                for (int WallId = 0; WallId < WALL_COUNT; ++WallId)
                {
                    int wallX = wallpositonsX[WallId];
                    int wallY = wallpositionsY[WallId];

                    for (int BoxId = 0; BoxId < BOX_COUNT; ++BoxId)
                    {
                        int boxX = boxpositionsX[BoxId];
                        int boxY = boxpositionsY[BoxId];

                        if (boxX == wallX && boxY == wallY)
                        {
                            switch (playerMoveDirection)
                            {
                                case Direction.Left:
                                    boxX = wallX + 1;
                                    playerX = boxX + 1;
                                    break;

                                case Direction.Right:
                                    boxX = wallX - 1;
                                    playerX = boxX - 1;
                                    break;

                                case Direction.Up:
                                    boxY = wallY + 1;
                                    playerY = boxY + 1;
                                    break;

                                case Direction.Down:
                                    boxY = wallY - 1;
                                    playerY = boxY - 1;
                                    break;
                            }

                            boxpositionsX[BoxId] = boxX;
                            boxpositionsY[BoxId] = boxY;
                        }
                    }
                }

                // 박스끼리 충돌처리
                for (int collidedBoxId = 0; collidedBoxId < BOX_COUNT; ++collidedBoxId)
                {
                    if (pushedBoxId == collidedBoxId)
                    {
                        continue;
                    }

                    if (boxpositionsX[pushedBoxId] == boxpositionsX[collidedBoxId] && boxpositionsY[pushedBoxId] == boxpositionsY[collidedBoxId])
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                boxpositionsX[pushedBoxId] = boxpositionsX[collidedBoxId] + 1;
                                playerX = boxpositionsX[pushedBoxId] + 1;
                                break;

                            case Direction.Right:
                                boxpositionsX[pushedBoxId] = boxpositionsX[collidedBoxId] - 1;
                                playerX = boxpositionsX[pushedBoxId] - 1;
                                break;

                            case Direction.Up:
                                boxpositionsY[pushedBoxId] = boxpositionsY[collidedBoxId] + 1;
                                playerY = boxpositionsY[pushedBoxId] + 1;
                                break;

                            case Direction.Down:
                                boxpositionsY[pushedBoxId] = boxpositionsY[collidedBoxId] - 1;
                                playerY = boxpositionsY[pushedBoxId] - 1;
                                break;
                        }
                    }
                }

                // 함정과 플레이어의 위치가 같을 때(플레이어의 위치가 처음 위치로 다시 돌아간다.)
                for (int i = 0; i < BAD_COUNT; ++i)
                {
                    if (playerX == baditempositionsX[i] && playerY == baditempositionsY[i])
                    {
                        playerX = 1;
                        playerY = 1;
                    }
                }                

                // 박스의 위치와 골의 위치가 같을때
                // 모든 골 지점에 박스가 올라와 있다.
                int count = 0;

                for (int i = 0; i < GOAL_COUNT; ++i)
                {
                    int goalX = goalpositionsX[i];
                    int goalY = goalpositionsY[i];

                    for (int j = 0; j < BOX_COUNT; ++j)
                    {
                        int boxX = boxpositionsX[j];
                        int boxY = boxpositionsY[j];

                        if (boxX == goalX && boxY == goalY)
                        {
                            ++count;

                            if (count == 2)
                            {
                                Console.Clear();
                                Console.WriteLine("\n\n    클리어~! 축하합니다!!");

                                return;
                            }
                        }
                    }
                }
            }
                        
            void RenderObject(int x, int y, string icon)                       
            {                           
                Console.SetCursorPosition(x, y);                           
                Console.Write(icon);                       
            }
        }
    }
}
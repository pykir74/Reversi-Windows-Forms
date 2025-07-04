using System;
using System.Collections.Generic;
using System.Media;

public class ReversiGame
{
    public const int Empty = 0;
    public const int Black = 1;
    public const int White = 2;
    public const int Green = 4;

    string s_clickSound;


    public int[,] Board { get; private set; } = new int[8, 8];
    public int CurrentPlayer { get; set; } = Black;

    private (int dx, int dy)[] directions = new (int, int)[]
    {
        (-1, -1), (-1, 0), (-1, 1),
        (0, -1),          (0, 1),
        (1, -1), (1, 0), (1, 1)
    };

    public ReversiGame(string clickSound)
    {
        // starting grid
        Board[3, 3] = White;
        Board[3, 4] = Black;
        Board[4, 3] = Black;
        Board[4, 4] = White;
        s_clickSound = clickSound;
    }

    public bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    public bool IsValidMove(int x, int y, int player)
    {
        if (!IsInsideBoard(x, y) || Board[x, y] != Empty)
            return false;

        int opponent = (player == Black) ? White : Black;

        foreach (var (dx, dy) in directions)
        {
            int cx = x + dx, cy = y + dy;
            bool hasOpponentBetween = false;

            while (IsInsideBoard(cx, cy) && Board[cx, cy] == opponent)
            {
                cx += dx;
                cy += dy;
                hasOpponentBetween = true;
            }

            if (hasOpponentBetween && IsInsideBoard(cx, cy) && Board[cx, cy] == player)
            {
                return true;
            }
        }

        return false;
    }

    public List<(int x, int y)> GetValidMoves(int player)
    {
        var moves = new List<(int x, int y)>();
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                if (IsValidMove(x, y, player))
                {
                    moves.Add((x, y));
                    //Board[x, y] = Green; 
                }
        return moves;
    }

    private void PlayClickSound()
    {
        try
        {
            SoundPlayer player = new SoundPlayer(s_clickSound);
            player.Play();
        }
        catch (Exception ex)
        {
            MessageBox.Show("err: cannot play a sound " + ex.Message);
        }
    }
    public bool ApplyMove(int x, int y)
    {
        if (!IsValidMove(x, y, CurrentPlayer))
            return false;

        Board[x, y] = CurrentPlayer;
        int opponent = (CurrentPlayer == Black) ? White : Black;

        foreach (var (dx, dy) in directions)
        {
            // cx = current checked x, cy = current checked y
            int cx = x + dx, cy = y + dy;
            var toFlip = new List<(int, int)>();

            while (IsInsideBoard(cx, cy) && Board[cx, cy] == opponent)
            {
                toFlip.Add((cx, cy));
                cx += dx;
                cy += dy;
            }

            if (IsInsideBoard(cx, cy) && Board[cx, cy] == CurrentPlayer)
            {
                foreach (var (fx, fy) in toFlip)
                    Board[fx, fy] = CurrentPlayer;
            }
        }
        PlayClickSound();
        SwitchPlayer();
        return true;
    }

    public void SwitchPlayer()
    {
        int other = (CurrentPlayer == Black) ? White : Black;
        if (GetValidMoves(other).Count > 0)
        {
            CurrentPlayer = other;
        }
        else if (GetValidMoves(CurrentPlayer).Count == 0)
        {
            // game over
            CurrentPlayer = 0;
        }
    }

    public (int x, int y)? GetRandomAIMove(int player)
    {
        var moves = GetValidMoves(player);
        if (moves.Count == 0)
            return null;
        var rand = new Random();
        return moves[rand.Next(moves.Count)];
    }

    public (int black, int white) GetScore()
    {
        int black = 0, white = 0;
        foreach (int cell in Board)
        {
            if (cell == Black) black++;
            else if (cell == White) white++;
        }
        return (black, white);
    }

    public string GetWinner()
    {
        int blackScore = 0, whiteScore = 0;
        foreach (int cell in Board)
        {
            if (cell == Black) blackScore++;
            else if (cell == White) whiteScore++;
        }
        if (blackScore > whiteScore)
            return "Black wins!";
        else if (whiteScore > blackScore)
            return "White wins!";
        else
            return "It's a draw!";
    }

    public void Restart()
    {
        Board = new int[8, 8];
        Board[3, 3] = White;
        Board[3, 4] = Black;
        Board[4, 3] = Black;
        Board[4, 4] = White;
        CurrentPlayer = Black;
    }

    public bool IsGameOver()
    {
        return GetValidMoves(Black).Count == 0 && GetValidMoves(White).Count == 0;
    }
    
}

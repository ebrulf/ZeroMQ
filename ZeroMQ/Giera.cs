using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace ZeroMQ
{
    public class Giera
    {
        public char[,] Board;
        public char Turn;
        public char You;
        public char Opponent;
        public char? Winner;
        public bool GameOver;
        public int Counter;
        public Giera(int n=3) 
        {
            if (n <= 2)
                n=3; //throw exception
            Board = new char[n,n];
            Board = Inicjalizuj(Board, ' ');
            Turn = 'X';
            You = 'X';
            Opponent = 'O';
            Winner = null;
            GameOver = false;
            Counter = 0;
        }
        public char[,] Inicjalizuj(char[,] table, char zero)
        {
            for(int i=0; i< table.GetLength(0); i++)
            {
                for(int j=0; j<table.GetLength(1); j++)
                {
                    table[i, j] = zero;
                }
            }
            return table;
        }
        public void HandleConnection(ResponseSocket klient)
        {
            int move;
            while(!GameOver)
            {
                if(Turn==You)
                {
                    move = PobierzRuch();
                    if(CheckValidMove(move))
                    {
                        ApplyMove(move, You);
                        Turn = Opponent;
                        klient.SendFrame(move.ToString());//wyślij klientowi ruch
                    }
                    else
                    {
                        Console.WriteLine("Niedozwolony ruch"); //to nie ma nawet być monit
                    }
                }
                else
                { 

                    string dane = klient.ReceiveFrameString(Encoding.UTF8);//góra 1024 bajty
                    if(dane is null)
                    {
                        //rozłącz się z klientem
                        break;
                    }
                    else
                    {
                        move = Convert.ToInt32(dane);//plus jeszcze jakieś sztuczki
                        ApplyMove(move, Opponent);
                        Turn = You;
                    }
                }
            }
            // w razie czego się rozłącz
        }
        public int PobierzRuch()
        {
            return 1;
        }
        public bool CheckValidMove(int move)
        {
            return false
        }
        public void ApplyMove(int move, char gracz)
        {

        }
    }
}

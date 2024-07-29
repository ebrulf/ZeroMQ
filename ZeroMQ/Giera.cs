using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private Random losuj;
        public Giera(int n=3) 
        {
            if (n <= 2)
                n=3; //throw exception
            Board = new char[n,n];
            Board = Inicjalizuj(Board, ' ');
            Turn = 'X';
            losuj = new Random();
            You = losuj.Next(2)==0? 'X': 'O'; //w ten sposób serwer nie będzie zawsze zaczynał
            Opponent = 'O';
            Winner = null;// albo ' '
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
            Tuple<int, int> move;
            while(!GameOver)
            {
                if(Turn==You)
                {
                    move = PobierzRuch();
                    if(CheckValidMove(move))
                    {
                        klient.SendFrame(move.ToString());//wyślij klientowi ruch w postaci "(pierwszy, drugi)"
                        ApplyMove(move, You);
                        Turn = Opponent;
                    }
                    else
                    {
                        Console.WriteLine("Niedozwolony ruch"); //to nie ma nawet być monit
                    }
                }
                else
                { 

                    string dane = klient.ReceiveFrameString(Encoding.UTF8);//góra 1024 bajty
                    string pattern = @"[0-9]+";
                    MatchCollection znajdzki = Regex.Matches(dane, pattern);
                    if (dane is null || znajdzki.Count != 2)
                    {
                        Console.WriteLine("Czemu liczb jest " + znajdzki.Count);
                        //rozłącz się z klientem
                        break; //tu też powinien być exception
                    }
                    else
                    {   
                        move= new Tuple<int,int>(Convert.ToInt32(znajdzki[0].Value), Convert.ToInt32(znajdzki[1].Value));//plus jeszcze jakieś sztuczki typu regex [0-9]+
                        ApplyMove(move, Opponent);
                        Turn = You;
                    }
                }
            }
            // w razie czego się rozłącz
        }
        public Tuple<int, int> PobierzRuch()
        {
            return new Tuple<int,int>(1, 1);
        }
        public bool CheckValidMove(Tuple<int, int> move)
        {
            return Board[move.Item1, move.Item2] == ' ';
        }
        public void ApplyMove(Tuple<int,int> move, char gracz)
        {
            if (GameOver)
                return;
            Counter += 1;
            Board[move.Item1, move.Item2] = gracz;
            PrintBoard();
            if(CheckForWin())
            {
                if (Winner == You)
                    Console.WriteLine("Wygrana");//nadpiszemy nagłówek drugiego stopnia
                else if (Winner == Opponent)
                    Console.WriteLine("Przegrana");
                //exit
            }
            else
            {
                if (Counter == Board.Length)
                {
                    Console.WriteLine("Remis");
                    //exit
                }
                    
            }
        }
        public void PrintBoard() //działa
        {
            //update stan GUI
            for (int i=0; i<Board.GetLength(0); i++)
            {
                for(int j=0; j<Board.GetLength(1); j++)
                {
                    if (j != 0)
                        Console.Write('|');//tylko z lewej strony daje
                    Console.Write(Board[i, j]);
                }
                Console.Write('\n');
                if (i != Board.GetLength(0) - 1)
                    for(int k=0; k<2*Board.GetLength(1)-1; k++)
                        Console.Write('-'); //tylko nie pod ostatnią liinjką
                Console.Write('\n');
            }
        }
        public bool CheckForWin()
        {
            //wiersze
            char sprawdz;
            //bool check; //mogę też nieefektywnie, a zrozumiale sprawdzać
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                int j = 0;
                sprawdz = Board[i, j];
                if (sprawdz == ' ')
                    continue;//ma sprawdzić następny wiersz
                for (; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j] != sprawdz)
                        break; //ma przestać sprawdzać w wierszu
                }
                if (Board[i,j]==sprawdz)//to powinno zadziałać w ostatniej kolumnie
                {
                    Winner = sprawdz;
                    GameOver = true;
                    return true;
                }
            }
            //kolumny
            for(int j = 0; j<Board.GetLength(1); j++)
            {
                int i = 0;
                sprawdz = Board[i, j];
                if (sprawdz == ' ')
                    continue;//ma sprawdzić następną kolumnę
                for (; i < Board.GetLength(0); i++)
                {
                    if (Board[i, j] != sprawdz)
                        break; //ma przestać sprawdzać w kolumnie
                }
                if (Board[i, j] == sprawdz)//sprawdzamy ostatni wiersz
                {
                    Winner = sprawdz;
                    GameOver = true;
                    return true;
                }
            }
            //przekątne
            sprawdz = Board[0, 0];
            int inn;
            for(inn=1; inn<Board.GetLength(0); inn++)
            {
                if (Board[inn, inn] != sprawdz)
                    break;
            }
            if(inn==Board.GetLength(0) && Board[inn,inn]==sprawdz && sprawdz!=' ')
            {
                Winner = sprawdz;
                GameOver = true;
                return true;
            }
            //i teraz druga
            sprawdz = Board[Board.GetLength(0) - 1, 0];
            for(inn = 1; inn<Board.GetLength(0); inn++)
            {
                if (Board[Board.GetLength(0)-1-inn, inn] != sprawdz)
                    break;
            }
            if (inn == Board.GetLength(0) && Board[Board.GetLength(0) - 1 - inn, inn] == sprawdz && sprawdz != ' ')
            {
                Winner = sprawdz;
                GameOver = true;
                return true;
            }
            return false;
        }
    }
}

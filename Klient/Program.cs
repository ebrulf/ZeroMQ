using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQ;
// trzeba zainstalować osobno do każdego rozwiązania
public static class Programik
{
    public static void Main()
    {
        Giera game = new Giera();
        Console.WriteLine("Łączenie z drugim graczem…");
        JoinGame(game);
        /*using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:5555");
            Random random = new Random(); //random.Next(100);
            Tuple<int, int> dwójka;
            int requestNumber;
            game.PrintBoard();
            for (requestNumber = 0; requestNumber != 10; requestNumber++)
            {
                dwójka = game.PobierzRuch(); //działa, kiedy działa
                //dwójka = new Tuple<int, int>(random.Next(100), random.Next(100));
                Console.WriteLine("Sending Hello {0}...", requestNumber);
                requester.SendFrame(dwójka.ToString());
                game.ApplyMove(dwójka, requestNumber%2==0?'X':'O');
                string str = requester.ReceiveFrameString();
                Console.WriteLine("Received World {0}. Suma: {1}", requestNumber, str);
                //game.PrintBoard();
            }
        }*/
        
    }

    public static void JoinGame(Giera game, string adres = "localhost", int port = 5555)
    {
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://"+adres+":"+port.ToString());
            game.You = 'O';
            game.Opponent = 'X';
            Tuple<int, int> move;
            Random random = new Random();
            game.Turn = 'X';
            //game.Turn = random.Next(2) == 0 ? 'X' : 'O';
            requester.SendFrame(game.Turn.ToString());
            //ten sam wątek do gry
            //game.HandleConnection(requester);//widzę problem, jak to związać w jeden wątek
            while (!game.GameOver && !requester.IsDisposed)
            {
                    Console.WriteLine("Ruch wykonuje gracz: " + game.Turn);//dla porządku
                    if (game.Turn == game.You)
                    {

                    Thread.BeginCriticalRegion(); 
                    move = game.PobierzRuch();
                    Thread.EndCriticalRegion();
                    if (game.CheckValidMove(move))
                        {
                            (requester).SendFrame(move.ToString());//wyślij klientowi ruch w postaci "(pierwszy, drugi)"
                            game.ApplyMove(move, game.You);
                            game.Turn = game.Opponent;
                        }
                        else
                        {
                            Console.WriteLine("Niedozwolony ruch"); //to nie ma nawet być monit
                        }
                        //requester.Poll();
                    }
                    else
                    {
                    Thread.Sleep(1000);
                        string dane = (requester).ReceiveFrameString(Encoding.UTF8);//góra 1024 bajty//wywaliło przez zmianę z ResponderSocket
                        string pattern = @"[0-9]+";
                        MatchCollection znajdzki = Regex.Matches(dane, pattern);
                        if (dane is null || znajdzki.Count != 2)
                        {
                            Console.WriteLine("Czemu liczb jest " + znajdzki.Count);
                            //rozłącz się z klientem
                            requester.Close(); //na razie to powinno wystarczyć
                            break; //tu też powinien być exception
                        }
                        else
                        {
                            move = new Tuple<int, int>(Convert.ToInt32(znajdzki[0].Value), Convert.ToInt32(znajdzki[1].Value));//plus jeszcze jakieś sztuczki typu regex [0-9]+
                            game.ApplyMove(move, game.Opponent);
                            game.Turn = game.You;
                        }
                    //requester.SignalOK(); //requester.Options.
                    }
            }
            requester.Close();
            Console.WriteLine("Papatki");
        }
    }


}
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQ;

public static class Programm
{
    public static void Main()
    {
        //PairSocket pairSocket = new PairSocket();//może?
        Giera game = new Giera();
        //HostGame
        Console.Write("Podaj adres IPv4 drugiego gracza (domyślnie: *): \n");
        //string add = Console.ReadLine(); //krzyczy o nullach, słusznie
        Console.Write("Podaj port połączenia TCP (domyślnie: 5555): \n");
        //string por = Console.ReadLine();
        //dalej oczyszczamy regexami wejścia, krzyczymy, jeśli są nieprawidłowe
        //HostGame(game, add, por);
        game.PrintBoard();
        HostGame(game);
        //zróbmy jednorazowo, bez rewanżu
        /*using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://*:5555");
            int a, b, licznik = 0;
            while (true)
            {
                string str = responder.ReceiveFrameString();
                Console.WriteLine("Otrzymano ruch.");
                string pattern = @"[0-9]+";
                MatchCollection znajdzki = Regex.Matches(str, pattern);
                Thread.Sleep(1000);  //  Do some 'work'
                Console.WriteLine("Otrzymano ({0}, {1})", a=Convert.ToInt32(znajdzki[0].Value), b=Convert.ToInt32(znajdzki[1].Value));
                responder.SendFrame((a+b).ToString());
                game.ApplyMove(new Tuple<int, int>(a, b), licznik++%2==0?'T':'N');

               // game.PrintBoard();
            }
        }*/
        /*DrugaPrzekątnaTest(3);
        DrugaPrzekątnaTest(5);*/
    }
    public static void DrugaPrzekątnaTest(int dlugosc)
    {
        Console.WriteLine("Liczymy przekątne dla {0}:", dlugosc);
        for (int inn = 0; inn < dlugosc ; inn++)
        {
            Console.WriteLine("{0} {1}", dlugosc - 1 - inn, inn);//powinno być lepiej
        }
    }
    //teraz za NeuralNine. Nie potrzeba do tego wątków
    // https://www.youtube.com/watch?v=s6HOPw_5XuY
    public static void HostGame(Giera game, string adres="*", int port=5555)
    {
        using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://"+adres+":"+port.ToString());
            //Thread.Sleep(1000);//słuchać
            //List<string> klient = responder.ReceiveMultipartStrings();//a nie Frame?, adres też pobieramy//ah, they were both bottoms
            game.You = 'X';
            game.Opponent = 'O';
            Tuple<int, int> move;
            //tu zaczynamy grę z klientem, funkcja handle_connection
            //Console.WriteLine(klient);
            //game.HandleConnection(responder);
            string d = responder.ReceiveFrameString();//once again
            if (d[0] != game.You && d[0] != game.Opponent)
                Console.WriteLine("Co znowu?");
            game.Turn = d[0];
            while(!game.GameOver && !responder.IsDisposed)
            {
                
                    Console.WriteLine("Ruch wykonuje gracz: " + game.Turn);//dla porządku
                    if (game.Turn == game.You)
                    {
                    Thread.BeginCriticalRegion();
                        move = game.PobierzRuch();
                    Thread.EndCriticalRegion();
                        if (game.CheckValidMove(move))//działa
                        {
                            (responder).SendFrame(move.ToString());//wyślij klientowi ruch w postaci "(pierwszy, drugi)"
                            game.ApplyMove(move, game.You);
                            game.Turn = game.Opponent;
                        }
                        else
                        {
                            Console.WriteLine("Niedozwolony ruch"); //to nie ma nawet być monit
                        }
                        //responder.R
                    }
                    else
                    {
                    Thread.Sleep(1000);
                        string dane = (responder).ReceiveFrameString(Encoding.UTF8);//góra 1024 bajty
                        string pattern = @"[0-9]+";
                        MatchCollection znajdzki = Regex.Matches(dane, pattern);
                        if (dane is null || znajdzki.Count != 2)
                        {
                            Console.WriteLine("Czemu liczb jest " + znajdzki.Count);
                            //rozłącz się z klientem
                            responder.Close(); //na razie to powinno wystarczyć
                            break; //tu też powinien być exception
                        }
                        else
                        {
                            move = new Tuple<int, int>(Convert.ToInt32(znajdzki[0].Value), Convert.ToInt32(znajdzki[1].Value));//plus jeszcze jakieś sztuczki typu regex [0-9]+
                            game.ApplyMove(move, game.Opponent);
                            game.Turn = game.You;
                        }
                    }
                
                // w razie czego się rozłącz
                //responder.Close(); //czy to działa jako//a!
            }
            //responder.Unbind("tcp://" + adres + ":" + port.ToString());//czemu responder is disposed?
            Console.WriteLine("Bajbaj");
        }
    }
}
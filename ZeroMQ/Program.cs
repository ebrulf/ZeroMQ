using System;
using System.Security.Cryptography.X509Certificates;
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
        //zróbmy jednorazowo, bez rewanżu
        using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://*:5555");
            int a, b, licznik = 0;
            while (true)
            {
                string str = responder.ReceiveFrameString();
                Console.WriteLine("Received Hello");
                string pattern = @"[0-9]+";
                MatchCollection znajdzki = Regex.Matches(str, pattern);
                Thread.Sleep(1000);  //  Do some 'work'
                Console.WriteLine("Otrzymano ({0}, {1})", a=Convert.ToInt32(znajdzki[0].Value), b=Convert.ToInt32(znajdzki[1].Value));
                responder.SendFrame((a+b).ToString());
                game.ApplyMove(new Tuple<int, int>(a, b), licznik++%2==0?'T':'N');
                game.PrintBoard();
            }
        }
    }
    //teraz za NeuralNine. Nie potrzeba do tego wątków
    // https://www.youtube.com/watch?v=s6HOPw_5XuY
    public static void HostGame(Giera game, string adres="*", int port=5555)
    {
        using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://"+adres+":"+port.ToString());
            Thread.Sleep(1000);
            List<string> klient = responder.ReceiveMultipartStrings();//a nie Frame?, adres też pobieramy
            //game.You = 'X';
            //game.Opponent = 'O';
            //tu zaczynamy grę z klientem, funkcja handle_connection
            game.HandleConnection(responder);
            responder.Unbind("tcp://" + adres + ":" + port.ToString());
        }
    }
}
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQ;

static class Program
{
    public static void Main()
    {
        Giera game = new Giera();
        //HostGame
        using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://*:5555");
            int a, b;
            while (true)
            {
                string str = responder.ReceiveFrameString();
                Console.WriteLine("Received Hello");
                string pattern = @"[0-9]+";
                MatchCollection znajdzki = Regex.Matches(str, pattern);
                Thread.Sleep(1000);  //  Do some 'work'
                Console.WriteLine("Otrzymano ({0}, {1})", a=Convert.ToInt32(znajdzki[0].Value), b=Convert.ToInt32(znajdzki[1].Value));
                responder.SendFrame((a+b).ToString());
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
            game.You = 'X';
            game.Opponent = 'O';
            //tu zaczynamy grę z klientem, funkcja handle_connection
            responder.Unbind("tcp://" + adres + ":" + port.ToString());
        }
    }
}
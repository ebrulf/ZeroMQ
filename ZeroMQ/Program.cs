using System;
using System.Security.Cryptography.X509Certificates;
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
            while (true)
            {
                string str = responder.ReceiveFrameString();
                Console.WriteLine("Received Hello");
                Thread.Sleep(1000);  //  Do some 'work'
                responder.SendFrame("World");
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
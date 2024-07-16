using System;
using NetMQ;
using NetMQ.Sockets;
// trzeba zainstalować osobno do każdego rozwiązania
static class Program
{
    public static void Main()
    {
        Console.WriteLine("Connecting to hello world server…");
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:5555");

            int requestNumber;
            for (requestNumber = 0; requestNumber != 10; requestNumber++)
            {
                Console.WriteLine("Sending Hello {0}...", requestNumber);
                requester.SendFrame("Hello");
                string str = requester.ReceiveFrameString();
                Console.WriteLine("Received World {0}", requestNumber);
            }
        }
    }
    public static void JoinGame(ZeroMQ.Giera game, string adres = "localhost", int port = 5555)
    {
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://"+adres+":"+port.ToString());
            game.You = 'O';
            game.Opponent = 'X';
            //ten sam wątek do gry
        }
    }


}
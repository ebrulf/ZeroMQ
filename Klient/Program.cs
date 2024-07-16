using System;
using NetMQ;
using NetMQ.Sockets;
// trzeba zainstalować osobno do każdego rozwiązania
static class Program
{
    public static void Main()
    {
        //Giera game = new Giera();
        Console.WriteLine("Connecting to hello world server…");
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:5555");
            Random random = new Random(); //random.Next(100);
            Tuple<int, int> dwójka;
            int requestNumber;
            for (requestNumber = 0; requestNumber != 10; requestNumber++)
            {
                dwójka = new Tuple<int, int>(random.Next(100), random.Next(100));
                Console.WriteLine("Sending Hello {0}...", requestNumber);
                requester.SendFrame(dwójka.ToString());
                string str = requester.ReceiveFrameString();
                Console.WriteLine("Received World {0}. Suma: {1}", requestNumber, str);
            }
        }
    }
    /*public static void JoinGame(ZeroMQ.Giera game, string adres = "localhost", int port = 5555)
    {
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://"+adres+":"+port.ToString());
            game.You = 'O';
            game.Opponent = 'X';
            //ten sam wątek do gry
        }
    }*/


}
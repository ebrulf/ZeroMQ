using System;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQ;
// trzeba zainstalować osobno do każdego rozwiązania
public static class Programik
{
    public static void Main()
    {
        Giera game = new Giera();
        Console.WriteLine("Connecting to hello world server…");
        //JoinGame(game);
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:5555");
            Random random = new Random(); //random.Next(100);
            Tuple<int, int> dwójka;
            int requestNumber;
            for (requestNumber = 0; requestNumber != 10; requestNumber++)
            {
                //dwójka = game.PobierzRuch(); //działa, kiedy działa
                dwójka = new Tuple<int, int>(random.Next(100), random.Next(100));
                Console.WriteLine("Sending Hello {0}...", requestNumber);
                requester.SendFrame(dwójka.ToString());
                //game.ApplyMove(dwójka, requestNumber%2==0?'X':'O');
                string str = requester.ReceiveFrameString();
                Console.WriteLine("Received World {0}. Suma: {1}", requestNumber, str);
                //game.PrintBoard();
            }
        }
    }
    public static void JoinGame(Giera game, string adres = "localhost", int port = 5555)
    {
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://"+adres+":"+port.ToString());
            game.You = 'O';
            game.Opponent = 'X';
            //ten sam wątek do gry
            //game.HandleConnection(requester);//widzę problem, jak to związać w jeden wątek
            while(!game.GameOver)
            {
                
            }
            requester.Close();
            Console.WriteLine("Papatki");
        }
    }


}
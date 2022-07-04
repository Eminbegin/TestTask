namespace Taskii
{
    class Program
    {
        private static Logger log = Logger.GetInstnce();

        static void Main()
        {
            Parser app = new Parser();
            app.SaveFromFile();
            log.FirstHelp();            
            String? input = Console.ReadLine();
            while (input != "/exit" && input != null)
            {
                app.Parse(input);
                input = Console.ReadLine();
            }

            app.SaveToFile();
        }
    }
}
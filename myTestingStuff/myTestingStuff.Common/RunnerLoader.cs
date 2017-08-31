using System.Threading.Tasks;

namespace myTestingStuff.Common
{
    public static class RunnerLoader
    {
        public static async Task<Runner> RunAsync() // we assume we return something after this operation 
        {
            Runner runner = new Runner();
            await Task.Run(() =>
            {
                runner.Run();
            }); //1 seconds delay
            return runner;
        }
    }
}

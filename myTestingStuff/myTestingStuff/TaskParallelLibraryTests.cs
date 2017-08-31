using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using myTestingStuff.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace thundax.myTestingStuff
{
    [TestClass]
    public class TaskParallelLibraryTests
    {
        [TestMethod]
        //Sequential execution
        //Runner1 -> Runner2 -> Runner3 -> Runner4
        public void TestLoadSequentially()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Runner> run = LoadRunnersSequentially(4);
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Assert.IsTrue(run.Count == 4);
            Assert.IsTrue(time > 4000);
        }

        [TestMethod]
        //Parallel execution
        //Runner1 -> Runner2 -> Runner3 -> Runner4
        public void TestLoadParallel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Runner> run = LoadRunnersInParallel(4);
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Assert.IsTrue(run.Count == 4);
            Assert.IsTrue(time < 4000);
        }

        [TestMethod]
        //Async and Await test
        public async Task TestLoadRunnersAsync()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Runner> run = await LoadRunnerAsync(4);
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Assert.IsTrue(run.Count == 4);
            Assert.IsTrue(time < 4000);
        }

        private async Task<List<Runner>> LoadRunnerAsync(int numberOfRunners)
        {
            var tasks = new List<Task<Runner>>();
            for (int i = 0; i < numberOfRunners; i++)
            {
                Task<Runner> task = RunnerLoader.RunAsync();
                tasks.Add(task);
            }

            Runner[] runners = await Task.WhenAll(tasks);
            return runners.ToList();
        }

        private List<Runner> LoadRunnersSequentially(int numberOfRunners)
        {
            var listRunners = new List<Runner>();
            for (int i = 0; i < numberOfRunners; i++)
            {
                Runner runner = new Runner();
                runner.Run();
                listRunners.Add(runner);
            }
            return listRunners;
        }

        private List<Runner> LoadRunnersInParallel(int numberOfRunners)
        {
            var listRunners = new BlockingCollection<Runner>();
            Parallel.For(0, numberOfRunners, i =>
            {
                Runner runner = new Runner();
                runner.Run();
                listRunners.Add(runner);
            });

            return listRunners.ToList();
        }

    }
}

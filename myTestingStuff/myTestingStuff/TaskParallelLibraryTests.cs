//  Copyright (c) 2017, Jordi Corbilla
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//
//  - Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//  - Neither the name of this library nor the names of its contributors may be
//    used to endorse or promote products derived from this software without
//    specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
//  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
//  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
//  INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
//  CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//  POSSIBILITY OF SUCH DAMAGE.

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

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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace thundax.myTestingStuff
{
    //Sample class to get results back from the thread
    public class ThreadExample
    {
        public string Argument1 { get; set; }
        public int Argument2 { get; set; }
    }

    [TestClass]
    public class ThreadPoolTests
    {
        [TestMethod]
        public void TestCallBack()
        {
            // Declare argument object.
            ThreadExample threadInfo = new ThreadExample();
            threadInfo.Argument1 = "string";
            threadInfo.Argument2 = 1;

            // Send the custom object to the threaded method.
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessExample), threadInfo);
        }

        [TestMethod]
        public void TestCallBackException()
        {
            // Declare argument object.
            ThreadExample threadInfo = new ThreadExample();
            threadInfo.Argument1 = "string";
            threadInfo.Argument2 = 1;

            // Send the custom object to the threaded method.
            //This should work as the exception will be swallowed
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessExampleException), threadInfo);
        }

        private void ProcessExample(object a)
        {
            // we can constrain the number of worker threads here if needed be.

            // We receive the ThreadExample as an object.
            // Use the 'as' operator to cast it to ThreadExample.
            ThreadExample threadInfo = a as ThreadExample;
            string argument = threadInfo.Argument1;
            int argument2 = threadInfo.Argument2;

            //Wait for a second
            Thread.Sleep(1000);
        }

        private void ProcessExampleException(object a)
        {
            // we can constrain the number of worker threads here if needed be.

            // We receive the ThreadExample as an object.
            // Use the 'as' operator to cast it to ThreadExample.
            ThreadExample threadInfo = a as ThreadExample;
            string argument = threadInfo.Argument1;
            int argument2 = threadInfo.Argument2;
            int value = 1 / int.Parse("0");
            //Wait for a second
            Thread.Sleep(1000);
        }
    }


}

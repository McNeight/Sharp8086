﻿#region License
// // The MIT License (MIT)
// // 
// // Copyright (c) 2016 Digital Singularity
// // 
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// // 
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharp8086.Core;
using Sharp8086.CPU;

namespace Sharp8086.Test
{
    [TestClass]
    public sealed class Cpu8086Test
    {
        [TestMethod]
        public void TestAdd()
        {
            ICpu cpu;
            using (var file = File.OpenRead("add"))
                cpu = new Cpu8086(file, 1024 * 1024);
            cpu.WriteBytes(0, new byte[0x100]);

            while (cpu.ProcessInstruction())
            {
            }

            using (var file = File.OpenRead("addResult"))
            {
                var goodData = new byte[file.Length];
                if (file.Read(goodData, 0, goodData.Length) != goodData.Length)
                    throw new InvalidDataException();

                var testData = cpu.ReadBytes(0, (uint)goodData.Length);

                var result = CompareArrays(goodData, testData);
                if (result != -1)
                    throw new InvalidOperationException();
            }
        }

        private static int CompareArrays<T>(IReadOnlyList<T> data1, IReadOnlyList<T> data2)
        {
            if (data1.Count != data2.Count)
                return 0;
            for (var i = 0; i < data1.Count; i++)
                if (!Equals(data1[i], data2[i]))
                    return i;
            return -1;
        }
    }
}
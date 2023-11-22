using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ModTests
{
    [TestClass()]
    public class TestClass1
    {
        [TestMethod()]
        public void fileMoveTest()
        {

            //  string csvFileName = Application.dataPath + "/abc.test";
            // File.Move(csvFileName, (long)System.DateTime.Now.Ticks / 10000 + csvFileName);
            Console.WriteLine((long)System.DateTime.Now.Ticks / 10000);

            long length = new System.IO.FileInfo("../../app.config").Length;
            string fileName = "../../test.json";
            Console.WriteLine(length); //字节 Byte
           // if (length > 10 * 1000 * 1024)
             
                string newFileName = fileName + "." + (long)System.DateTime.Now.Ticks / 10000;
                File.Move(fileName, newFileName);
                File.Move(newFileName,fileName );
          
            File.Copy(fileName, newFileName );
             
        }

    }
}

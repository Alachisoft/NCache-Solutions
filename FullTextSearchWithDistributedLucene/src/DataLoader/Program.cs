// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using NCommerce.FTSEngine;
using System;
using System.Diagnostics;

namespace NCommerce.DataLoader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //--- Go to the menu, select an option to perfrom!
                AppMenu();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }

            Console.WriteLine(Environment.NewLine + "Press ENTER key to exit ...");
            Console.ReadLine();
        }

        private static void AppMenu()
        {
            bool indexCreated = false;
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("DataLoader for Full-Text Search Demo of NCache with Lucene");
            var indexManager = FTSFactory.GetIndexManager();

            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo cki;
            do
            {
                Console.WriteLine(Environment.NewLine + "Please select an option:");
                Console.WriteLine("L >> Load Documents");
                Console.WriteLine("B >> Load Documents In Bulk");
                Console.WriteLine("U >> Update Documents");
                Console.WriteLine("D >> Delete Documents");
                Console.WriteLine("");
                Console.WriteLine("Q >> Quit");

                cki = Console.ReadKey(false); // show the key as you read it
                Console.WriteLine();

                try
                {
                    var count = 0;
                    switch (cki.KeyChar.ToString().ToLowerInvariant())
                    {
                        case "l"://--- L(l)oad documents from data source and add into NCache
                            if (indexCreated)// to stop creating adding duplicate documents in same app session
                            {
                                Console.WriteLine("Index already created by this instance of app!");
                                break;
                            }

                            stopwatch.Restart();

                            count = indexManager.CreateIndex();

                            stopwatch.Stop();
                            Console.WriteLine(count + " document indexed milli sec: " + stopwatch.ElapsedMilliseconds);
                            indexCreated = true;
                            break;

                        case "b"://--- Load documents in B(b)ulk from data source and add into NCache
                            if (indexCreated)// to stop creating adding duplicate documents in same app session
                            {
                                Console.WriteLine("Index already created by this instance of app!");
                                break;
                            }

                            stopwatch.Restart();

                            count = indexManager.CreateIndexInBulk();

                            stopwatch.Stop();
                            Console.WriteLine(count + " document indexed milli sec: " + stopwatch.ElapsedMilliseconds);
                            indexCreated = true;
                            break;

                        case "u"://--- Update documents in Lucene & NCache

                            stopwatch.Restart();

                            indexManager.Update("ACCEAZCV5BHHDHFK");

                            stopwatch.Stop();
                            Console.WriteLine("Time taken to UPDATE docs with indexes in milli sec: " + stopwatch.ElapsedMilliseconds);
                            break;

                        case "d"://--- Delete all stored documents in Lucene & NCache

                            stopwatch.Restart();

                            indexManager.DeleteAll();//It will delete all created documents and index

                            stopwatch.Stop();
                            Console.WriteLine("Time taken to DELETE docs with indexes in milli sec: " + stopwatch.ElapsedMilliseconds);
                            indexCreated = false;
                            break;

                        case "q":
                            return;

                        default:

                            Console.Error.WriteLine("Incorrection input provided! " + cki.KeyChar.ToString());
                            break;
                    }
                }
                catch (Exception exp)
                {
                    Console.Error.WriteLine(exp.ToString());
                    //throw;
                }
            }
            while (cki.Key != ConsoleKey.Escape);
        }
    }
}

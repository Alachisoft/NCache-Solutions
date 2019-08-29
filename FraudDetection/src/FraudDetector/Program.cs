// Copyright (c) 2019 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Configuration;

namespace FraudDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string cache = ConfigurationManager.AppSettings["CacheID"];

                if (string.IsNullOrEmpty(cache))
                {
                    Console.WriteLine("The CacheID cannot be null or empty.");
                    return;
                }

                FraudDetection detection = new FraudDetection();
                detection.StartDetection(cache);
                // dispose manager
                detection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
  
    }
}

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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PersistentNCache.Logging
{
    /// <summary>
    /// To log in FileLogger 
    /// </summary>
    class FileLogger : IProviderLogger
    {
        StreamWriter _outputFile = new StreamWriter("PersistenceStore.Log");
        public void LogError(string message)
        {
            _outputFile.WriteLine(message + ":Error");
        }

        public void LogWarning(string message)
        {
            _outputFile.WriteLine(message + ":Warning");
        }
    }
}

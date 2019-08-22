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

using Alachisoft.NCache.Common;
using PersistenceProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PersistentNCache
{
    public class Util
    {
        /// <summary>
        /// Get an instance using reflection from deployed assemblies
        /// </summary>
        /// <param name="fullyQualifiedName"></param>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static IPersistenceProvider CreateInstanceWithReflection(string fullyQualifiedName,string cacheName)
        {
            const string qualifiedInterfaceName = "PersistenceProviders.IPersistenceProvider";
            var interfaceFilter = new TypeFilter(InterfaceFilter);

            var path = AppUtil.DeployedAssemblyDir+cacheName;

            var di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles("*.dll"))
            {
                try
                {
                    //ReflectionOnlyLoadFrom
                   var nextAssembly = Assembly.LoadFrom(file.FullName);
                    try
                    {
                        foreach (var type in nextAssembly.GetTypes())
                        {
                            var myInterfaces = type.FindInterfaces(interfaceFilter, qualifiedInterfaceName);
                            foreach(var intrfce in myInterfaces)
                            {
                                if (type.AssemblyQualifiedName.Equals(fullyQualifiedName))
                                {

                                    return (IPersistenceProvider)Activator.CreateInstance(type);
                                }
                            }
                        }
                    }
                    catch
                    {
                        //ignore and wait for loading other files
                    }
                }
                catch (BadImageFormatException)
                {
                    throw;
                }
            }
            throw new FileNotFoundException("No dll loaded with FQN provided in cache configuration");
        }
        private static bool InterfaceFilter(Type typeObj, Object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

    }
}

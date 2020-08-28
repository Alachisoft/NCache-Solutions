//Copyright(c) 2014 Stack Exchange

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

//===============================================

using System;

namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// When performing a range query, by default the start / stop limits are inclusive;
    /// however, both can also be specified separately as exclusive
    /// </summary>
    [Flags]
    public enum Exclude
    {
        /// <summary>
        /// Both start and stop are inclusive
        /// </summary>
        None = 0,
        /// <summary>
        /// Start is exclusive, stop is inclusive
        /// </summary>
        Start = 1,
        /// <summary>
        /// Start is inclusive, stop is exclusive
        /// </summary>
        Stop = 2,
        /// <summary>
        /// Both start and stop are exclusive
        /// </summary>
        Both = Start | Stop
    }
}

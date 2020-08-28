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

namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// Represents a block of operations that will be sent to the server together;
    /// this can be useful to reduce packet fragmentation on slow connections - it
    /// can improve the time to get *all* the operations processed, with the trade-off
    /// of a slower time to get the *first* operation processed; this is usually
    /// a good thing. Unless this batch is a <b>transaction</b>, there is no guarantee
    /// that these operations will be processed either contiguously or atomically by the server.
    /// </summary>
    public interface IBatch : IDatabaseAsync
    {
        /// <summary>
        /// Execute the batch operation, sending all queued commands to the server.
        /// Note that this operation is neither synchronous nor truly asyncronous - it
        /// simply enqueues the buffered messages. To check on completion, you should
        /// check the individual responses.
        /// </summary>
        void Execute();
    }
}

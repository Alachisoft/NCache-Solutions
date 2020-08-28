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

using System.Threading.Tasks;

namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// Represents a group of operations that will be sent to the server as a single unit,
    /// and processed on the server as a single unit. Transactions can also include constraints
    /// (implemented via WATCH), but note that constraint checking involves will (very briefly)
    /// block the connection, since the transaction cannot be correctly committed (EXEC),
    /// aborted (DISCARD) or not applied in the first place (UNWATCH) until the responses from
    /// the constraint checks have arrived.
    /// </summary>
    /// <remarks>https://redis.io/topics/transactions</remarks>
    /// <remarks>Note that on a cluster, it may be required that all keys involved in the transaction
    /// (including constraints) are in the same hash-slot</remarks>
    public interface ITransaction : IBatch
    {
        /// <summary>
        /// Adds a precondition for this transaction
        /// </summary>
        /// <param name="condition">The condition to add to the transaction.</param>
        ConditionResult AddCondition(Condition condition);

        /// <summary>
        /// Execute the batch operation, sending all queued commands to the server.
        /// </summary>
        /// <param name="flags">The command flags to use.</param>
        bool Execute(CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// Execute the batch operation, sending all queued commands to the server.
        /// </summary>
        /// <param name="flags">The command flags to use.</param>
        Task<bool> ExecuteAsync(CommandFlags flags = CommandFlags.None);
    }
}

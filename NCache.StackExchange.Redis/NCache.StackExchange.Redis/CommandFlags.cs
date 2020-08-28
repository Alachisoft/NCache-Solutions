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
using System.ComponentModel;

namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// Behaviour markers associated with a given command
    /// </summary>
    [Flags]
    public enum CommandFlags
    {
        /// <summary>
        /// Default behaviour.
        /// </summary>
        None = 0,

        /// <summary>
        /// From 2.0, this flag is not used
        /// </summary>
        [Obsolete("From 2.0, this flag is not used", false)]
        HighPriority = 1,
        /// <summary>
        /// The caller is not interested in the result; the caller will immediately receive a default-value
        /// of the expected return type (this value is not indicative of anything at the server).
        /// </summary>
        FireAndForget = 2,

        /// <summary>
        /// This operation should be performed on the master if it is available, but read operations may
        /// be performed on a replica if no master is available. This is the default option.
        /// </summary>
        PreferMaster = 0,

        /// <summary>
        /// This operation should only be performed on the master.
        /// </summary>
        DemandMaster = 4,



        /// <summary>
        /// This operation should be performed on the replica if it is available, but will be performed on
        /// a master if no replicas are available. Suitable for read operations only.
        /// </summary>
        [Obsolete("Starting with Redis version 5, Redis has moved to 'replica' terminology. Please use " + nameof(PreferReplica) + " instead.")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        PreferSlave = 8,

        /// <summary>
        /// This operation should be performed on the replica if it is available, but will be performed on
        /// a master if no replicas are available. Suitable for read operations only.
        /// </summary>
        PreferReplica = 8, // note: we're using a 2-bit set here, which [Flags] formatting hates; position is doing the best we can for reasonable outcomes here

        /// <summary>
        /// This operation should only be performed on a replica. Suitable for read operations only.
        /// </summary>
        DemandReplica = 12, // note: we're using a 2-bit set here, which [Flags] formatting hates; position is doing the best we can for reasonable outcomes here

        /// <summary>
        /// This operation should only be performed on a replica. Suitable for read operations only.
        /// </summary>
        [Obsolete("Starting with Redis version 5, Redis has moved to 'replica' terminology. Please use " + nameof(DemandReplica) + " instead.")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        DemandSlave = 12,

        // 16: reserved for additional "demand/prefer" options

        // 32: used for "asking" flag; never user-specified, so not visible on the public API

        /// <summary>
        /// Indicates that this operation should not be forwarded to other servers as a result of an ASK or MOVED response
        /// </summary>
        NoRedirect = 64,

        // 128: used for "internal call"; never user-specified, so not visible on the public API

        // 256: used for "script unavailable"; never user-specified, so not visible on the public API

        /// <summary>
        /// Indicates that script-related operations should use EVAL, not SCRIPT LOAD + EVALSHA
        /// </summary>
        NoScriptCache = 512,

        // 1024: used for timed-out; never user-specified, so not visible on the public API
    }
}

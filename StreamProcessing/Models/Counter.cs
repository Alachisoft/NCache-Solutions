using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    [Serializable]
    public class Counter : EntityBase
    {
        public long Value { get; set; } = -1L;
    }
}

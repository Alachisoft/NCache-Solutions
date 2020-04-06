using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    [Serializable]
    public class EntityBase
    {
        public string Id { get; set; }
    }
}

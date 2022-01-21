using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelLayer
{
    public class AddressModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string addressID { get; set; }

        [ForeignKey("RegisterModel")]
        public string userId { get; set; }
        public virtual RegisterModel RegisterModel { get; set; }

        [ForeignKey("AddressType")]
        public string addTypeId { get; set; }
        public string fullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double pinCode { get; set; }


    }
}

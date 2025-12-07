using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarWorkshopAPI.Models
{
 
    public class Garage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // MongoDB unique identifier

        [BsonElement("mispar_mosah")]
        public int MisparMosah { get; set; }  // garage number

        [BsonElement("shem_mosah")]
        public string ShemMosah { get; set; }  // garage name

        [BsonElement("cod_sug_mosah")]
        public int CodSugMosah { get; set; }  // garage type code

        [BsonElement("sug_mosah")]
        public string SugMosah { get; set; }  // garage type name

        [BsonElement("ktovet")]
        public string Ktovet { get; set; }  // address

        [BsonElement("yishuv")]
        public string Yishuv { get; set; }  // city

        [BsonElement("telephone")]
        public string Telephone { get; set; }  // phone number

        [BsonElement("mikud")]
        public int Mikud { get; set; }  // postal code

        [BsonElement("cod_miktzoa")]
        public int CodMiktzoa { get; set; }  // profession code

        [BsonElement("miktzoa")]
        public string Miktzoa { get; set; }  // profession name

        [BsonElement("menahel_miktzoa")]
        public string MenahelMiktzoa { get; set; }  // profession manager

        [BsonElement("rasham_havarot")]
        public long RashamHavarot { get; set; }  // vehicle registration number

        [BsonElement("TESTIME")]
        public string Testime { get; set; }  // test time 
    }
}

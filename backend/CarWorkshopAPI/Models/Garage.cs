using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarWorkshopAPI.Models
{
    public class Garage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("mispar_mosah")]
        public int MisparMosah { get; set; }

        [BsonElement("shem_mosah")]
        public string ShemMosah { get; set; }

        [BsonElement("cod_sug_mosah")]
        public int CodSugMosah { get; set; }

        [BsonElement("sug_mosah")]
        public string SugMosah { get; set; }

        [BsonElement("ktovet")]
        public string Ktovet { get; set; }

        [BsonElement("yishuv")]
        public string Yishuv { get; set; }

        [BsonElement("telephone")]
        public string Telephone { get; set; }

        [BsonElement("mikud")]
        public int Mikud { get; set; }

        [BsonElement("cod_miktzoa")]
        public int CodMiktzoa { get; set; }

        [BsonElement("miktzoa")]
        public string Miktzoa { get; set; }

        [BsonElement("menahel_miktzoa")]
        public string MenahelMiktzoa { get; set; }

        [BsonElement("rasham_havarot")]
        public long RashamHavarot { get; set; }

        [BsonElement("TESTIME")]
        public string Testime { get; set; }
    }
}

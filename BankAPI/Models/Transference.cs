using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankAPI.Models
{
    public class Transference
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string SourceWalletId { get; set; }
        public string TargetWalletId { get; set; }
        public string Value { get; set; }
        public string ValuePlusFee { get; set; }
        public string SourceWalletCurrency { get; set; }
        public string TargetWalletCurrency { get; set; }
        public string FeeCharged { get; set; }
        public DateTime? Date { get; set; }


        public Transference(string sourceWalletId, string targetWalletId, string value, string valuePlusFee, string sourceWalletCurrency, string targetWalletCurrency, string feeCharged, DateTime date)
        {
            SourceWalletId = sourceWalletId;
            TargetWalletId = targetWalletId;
            Value = value;
            ValuePlusFee = valuePlusFee;
            SourceWalletCurrency =  sourceWalletCurrency;
            TargetWalletCurrency = targetWalletCurrency;
            FeeCharged = feeCharged;
            Date = DateTime.Now;
        }
    }

    public class TransferencePayload
    {
        public string SourceWalletId { get; set; }
        public string TargetWalletId { get; set; }
        public string Value { get; set; }
        public string ValuePlusFee { get; set; }
        public string SourceWalletCurrency { get; set; }
        public string TargetWalletCurrency { get; set; }
        public string FeeCharged { get; set; }
    }

    public class DepositPayload
    {
        public string WalletId { get; set; }
        public string Value { get; set; }
        public string ValuePlusFee { get; set; }
        public string FeeCharged { get; set; }
    }
}

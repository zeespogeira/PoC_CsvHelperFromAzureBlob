using CsvHelper.Configuration;

namespace PoC_ParseMultipleCSVFiles
{
    public class ServiceCov
    {
        public string ContractNumber { get; set; }
        public string PremiseNumber { get; set; }
        public string ProductCode { get; set; }
        public string ServiceQuantity { get; set; }
        public string ServiceBranchNumber { get; set; }
    }

    public class ServiceCovMap : ClassMap<ServiceCov>
    {
        public ServiceCovMap()
        {
            AutoMap();
            Map(m => m.ContractNumber).Index(0);
            Map(m => m.PremiseNumber).Index(1);
            Map(m => m.ProductCode).Index(2);
            Map(m => m.ServiceQuantity).Index(3);
            Map(m => m.ServiceBranchNumber).Index(4);
        }
    }
}
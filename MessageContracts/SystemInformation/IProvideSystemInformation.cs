using System.ServiceModel;

namespace MessageContracts.SystemInformation
{
    [ServiceContract(Name = "SystemInformationService", Namespace = "http://waypoint.ifint.biz/systemInformation")]
    public interface IProvideSystemInformation
    {
        [OperationContract]
        Response GetSystemInformation(Request informationRequest);
    }
}
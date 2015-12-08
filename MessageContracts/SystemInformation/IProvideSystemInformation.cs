namespace MessageContracts.SystemInformation
{
    public interface IProvideSystemInformation
    {
        Response GetSystemInformation(Request informationRequest);
    }
}
using DataminersModel.ViewModels;


namespace DataminersDAL.Repositories
{
    public interface IITDBCheckerRepository
    {
        List<RequestDetailsOpenWIPViewModel> RequestDetailsOpenWIPs(string NtloginID);

        List<RequestsearchViewModel> requestsearchViewModels(string requestID);
    }
}

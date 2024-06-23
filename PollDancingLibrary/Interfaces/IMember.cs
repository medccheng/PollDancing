using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PollDancingLibrary.Interfaces
{
    public interface IMember
    {
        Task<ActionResult> GetCount();
        Task<ActionResult> GetAll(int page = 1);
        Task<ActionResult> GetDetail(int memberId);

        Task<ActionResult> GetAll(int page = 1, string search=null);
    }
}

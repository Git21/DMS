using DocumentManagementSystem.Interfaces;
using System.Linq;

namespace DocumentManagementSystem.Static
{
    public static class Helper
    {
        public static int? GetLatestDisplayOrder(IRepository repo)
        {
            return repo.GetDocs().
                 OrderByDescending(x => x.DisplayOrder).Take(1).
                 FirstOrDefault()?.DisplayOrder;
        }
    }
}

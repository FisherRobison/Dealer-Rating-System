using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Web.Services.Interfaces
{
    public interface IRatingsService
    {
        int InsertRating(RatingRequestModel model);
        bool hasRated(int? dealerID);
        List<Domain.Dealer> GetPaginationByUserId(PaginateListRequestModel model, int UserId);
        List<Domain.Dealer> GetByUserId(int UserId);
        int getRatingCountByDealerId(int UserId);
    }
}

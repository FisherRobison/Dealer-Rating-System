using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/Ratings")]
    public class RatingsApiController : ApiController
    {
        private IRatingsService _RatingsService { get; set; }

        public RatingsApiController(IRatingsService RatingsService)
        {
            _RatingsService = RatingsService;
        }

        [Route("Insert"), HttpPost]
        public HttpResponseMessage RatingSubmit(RatingRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _RatingsService.InsertRating(model);
            return Request.CreateResponse(response);
        }


        [Route("{dealerId:int}"), HttpGet]
        public HttpResponseMessage GetDealerById(int dealerId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<Domain.Dealer> response = new ItemsResponse<Domain.Dealer>();

            response.Items = _RatingsService.GetByUserId(dealerId);

            return Request.CreateResponse(response);
        }



        [Route("list/{dealerId:int}"), HttpGet]
        public HttpResponseMessage GetRatingPaginationList([FromUri] PaginateListRequestModel model, int dealerId)
        {


            if (model == null)
            {
                model = new PaginateListRequestModel();

                model.CurrentPage = 1;
                model.ItemsPerPage = 10;

            }

            PaginateItemsResponse<Domain.Dealer> response = new PaginateItemsResponse<Domain.Dealer>();

            response.Items = _RatingsService.GetPaginationByUserId(model, dealerId);

            response.CurrentPage = model.CurrentPage;

            response.ItemsPerPage = model.ItemsPerPage;

            response.TotalItems = _RatingsService.getRatingCountByDealerId(dealerId);

            return Request.CreateResponse(response);
        }



    }
}

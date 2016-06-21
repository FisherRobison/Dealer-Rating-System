using Spright.Data;
using Spright.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Spright.Web.Domain;
using Spright.Web.Models.Requests.EVA;
using Spright.Web.Services.Interfaces;

namespace Spright.Web.Services
{
    public class RatingsService : BaseService, IRatingsService
    {

        public int InsertRating(RatingRequestModel model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Ratings_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());
                   paramCollection.AddWithValue("@Rating", model.Rating);
                   paramCollection.AddWithValue("@Comments", model.Comments);
                   paramCollection.AddWithValue("@TypeId", 1);
                   paramCollection.AddWithValue("@TargetId", model.dealerID);


                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;
                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               });
            return uid;
        }



        public bool hasRated(int? dealerID)
        {
            bool p = false;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Dealer_hasRated"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@userId", UserService.GetCurrentUserId());
                   paramCollection.AddWithValue("@targetId", dealerID);


               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0;
                   p = reader.GetSafeBool(startingIndex++);
               });

            return p;
        }



        public List<Domain.Dealer> GetPaginationByUserId(PaginateListRequestModel model, int UserId)
        {
            List<Domain.Dealer> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Rating_Select_Page"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                   paramCollection.AddWithValue("@TargetId", UserId);

               }
               , map: (Action<IDataReader, short>)delegate (IDataReader reader, short set)
               {
                   Domain.Dealer p = new Domain.Dealer();
                   int startingIndex = 0;

                   p.Rating = reader.GetSafeInt32(startingIndex++);
                   p.Comments = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.Dealer>();
                   }

                   list.Add(p);
               });

            return list;
        }

        public List<Domain.Dealer> GetByUserId(int UserId)
        {
            List<Domain.Dealer> list = new List<Domain.Dealer>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Ratings_SelectByTargetId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@targetId", UserId);

               }, map: (Action<IDataReader, short>)delegate (IDataReader reader, short set)
               {
                   Domain.Dealer p = new Domain.Dealer();

                   int startingIndex = 0;

                   p.Rating = reader.GetSafeInt32(startingIndex++);
                   p.Comments = reader.GetSafeString(startingIndex++);

                   list.Add(p);

               });

            return list;
        }


        public int getRatingCountByDealerId(int UserId)
        {
            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Ratings_SelectByCount"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@targetId", UserId);

               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0;

                   count = reader.GetSafeInt32(startingIndex++);

               });

            return count;
        }

    }
}

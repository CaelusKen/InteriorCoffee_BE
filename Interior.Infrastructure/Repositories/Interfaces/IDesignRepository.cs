﻿using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IDesignRepository
    {
        Task<(List<Design>, int)> GetDesignsAsync();
        Task<Design> GetDesignById(string id);
        Task CreateDesign(Design design);
        Task UpdateDesign(Design design);
        Task DeleteDesign(string id);

        #region Get Function
        Task<Design> GetDesign(Expression<Func<Design, bool>> predicate = null,
                                 Expression<Func<Design, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetDesign<TResult>(Expression<Func<Design, TResult>> selector,
                                          Expression<Func<Design, bool>> predicate = null,
                                          Expression<Func<Design, object>> orderBy = null, bool isAscend = true);
        Task<List<Design>> GetDesignList(Expression<Func<Design, bool>> predicate = null,
                                           Expression<Func<Design, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetDesignList<TResult>(Expression<Func<Design, TResult>> selector,
                                                    Expression<Func<Design, bool>> predicate = null,
                                                    Expression<Func<Design, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<Design>> GetDesignPagination(Expression<Func<Design, bool>> predicate = null,
                                                      Expression<Func<Design, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetDesignPagination<TResult>(Expression<Func<Design, TResult>> selector,
                                                               Expression<Func<Design, bool>> predicate = null,
                                                               Expression<Func<Design, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}

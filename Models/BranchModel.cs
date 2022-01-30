﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Models
{
    public class BranchModel
    {
        private AppDbContext _db;
        public BranchModel(AppDbContext context)
        {
            _db = context;
        }
        
        public List<Branch> GetThreeClosestStores(float? lat, float? lng)
        {
            List<Branch> branchDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Branches.FromSql("dbo.pGetThreeClosestStores @lat, @lng", latParam, lngParam);
                branchDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return branchDetails;
        }
    }
}

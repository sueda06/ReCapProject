using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, NewDatabaseContext>, IRentalDal
    {
        public List<RentalDetailDto> GetRentalDetails()
        {
            using (NewDatabaseContext context = new NewDatabaseContext())
            {
                var result = from r in context.Rentals
                             join c in context.Cars
                             on r.CarId equals c.Id

                             join b in context.Brands
                             on c.BrandId equals b.BrandId

                             join custo in context.Customers
                             on r.CustomerId equals custo.UserId

                             join user in context.Users
                             on custo.UserId equals user.Id
                             select new RentalDetailDto
                             {
                                 BrandName = b.BrandName,
                                 Id = r.Id,
                                 Name=user.FirstName + user.LastName,
                                 RentDate=r.RentDate,
                                 ReturnDate =r.ReturnDate
                             };
                return result.ToList();
            }
        }
    }
}

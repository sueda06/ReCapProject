using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Linq;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, NewDatabaseContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (NewDatabaseContext context = new NewDatabaseContext())
            {
                var result = from c in filter == null ? context.Cars : context.Cars.Where(filter)
                             join co in context.Colors
                             on c.ColorId equals co.colorId
                             join b in context.Brands
                             on c.BrandId equals b.BrandId
                             select new CarDetailDto
                             {
                                 Description = c.Description,
                                 BrandId=b.BrandName,
                                 ColorId=co.ColorName,
                                 DailyPrice=c.DailyPrice,
                                 ModelYear=c.ModelYear,
                                 Id=c.Id,
                                 ImagePath= context.CarImages.Where(x => x.CarId == c.Id).FirstOrDefault().ImagePath
                             };
                  return result.ToList();
            }
        }
    }
}

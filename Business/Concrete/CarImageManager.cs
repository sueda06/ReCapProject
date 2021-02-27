using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        
        public IResult Add(IFormFile file, CarImage carImage)
        {
            var result= BusinessRules.Run(CheckNumberOfPictures(carImage.CarId));
            if(result != null)
            {
                return result;
            }

            carImage.ImagePath = FileHelper.AddAsync(file);
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);
            return new SuccessResult();
        }

        public IResult Delete(CarImage carImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _carImageDal.Get(p => p.Id == carImage.Id).ImagePath;

            IResult result = BusinessRules.Run(
                FileHelper.DeleteAsync(oldpath));

            if (result != null)
            {
                return result;
            }

            _carImageDal.Delete(carImage);
            return new SuccessResult();
        }

        public IDataResult<CarImage> Get(int Id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(p => p.Id == Id));
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll());
        }

        public IDataResult<List<CarImage>> GetImagesByCarId(int carId)
        {
            IResult result = BusinessRules.Run(CheckIfCarImageNull(carId));

            if (result != null)
            {
                return new ErrorDataResult<List<CarImage>>(result.Message);
            }

            return new SuccessDataResult<List<CarImage>>(CheckIfCarImageNull(carId).Data);
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _carImageDal.Get(p => p.Id == carImage.Id).ImagePath;
            carImage.ImagePath = FileHelper.UpdateAsync(oldpath, file);
            carImage.Date = DateTime.Now;
            _carImageDal.Update(carImage);
            return new SuccessResult();
        }

        private IResult CheckNumberOfPictures(int CarId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == CarId).Count;
            if(result >= 5)
            {
                return new ErrorResult(Messages.CarImageLımıtError);
            }
            return new SuccessResult();
     
        }

        private IDataResult<List<CarImage>> CheckIfCarImageNull(int id)
        {
            try
            {
                string path = @"\Images\default.jpg";
                var result = _carImageDal.GetAll(c => c.CarId == id).Any();
                if (!result)
                {
                    List<CarImage> carimage = new List<CarImage>();
                    carimage.Add(new CarImage { CarId = id, ImagePath = path, Date = DateTime.Now });
                    return new SuccessDataResult<List<CarImage>>(carimage);
                }
            }
            catch (Exception exception)
            {

                return new ErrorDataResult<List<CarImage>>(exception.Message);
            }

            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(p => p.CarId == id).ToList());
        }
    }
}

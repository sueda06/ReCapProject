﻿using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Performance;
using Core.Aspect.Autofac.Transaction;
using Core.Aspect.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Business.Concrete
{
    public class CarManager :ICarService
    {
        ICarDal _carDal;
        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }


        [SecuredOperation("car.add")]
        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Add(Car car)
        {
                _carDal.Add(car);
                return new SuccessResult(Messages.Added);
            
        }


        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Car car)
        {
            Add(car);
            if (car.DailyPrice < 50)
            {
                throw new Exception();
            }
            Add(car);
            return new SuccessResult();
        }

        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.Deleted);
        }

        [CacheAspect]
        [PerformanceAspect(0)]
        public IDataResult<List<Car>> GetAll()
        {
            Thread.Sleep(5000);
            return new SuccessDataResult<List<Car>>( _carDal.GetAll(),Messages.Listed);
        }

        [CacheAspect]
        [PerformanceAspect(1)]
        public IDataResult<Car> GetById(int Id)
        {
            return new SuccessDataResult<Car>( _carDal.Get(c => c.Id == Id), Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetail(int Id)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(c => c.Id == Id));
        }

        public IDataResult <List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>> (_carDal.GetCarDetails(), Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarsByBrandId(int Id)
        {
           return new SuccessDataResult<List<CarDetailDto>>( _carDal.GetCarDetails(c => c.BrandId == Id), Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarsByColorId(int Id)
        {
            return new SuccessDataResult<List<CarDetailDto>>( _carDal.GetCarDetails(c => c.ColorId == Id), Messages.Listed);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.Updated);
        }
    }
}

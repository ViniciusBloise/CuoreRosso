using System;
using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace CuoreRosso.Dal
{
    public interface IModelDto { };
    public class ModelDto : IModelDto{ };
    public class HealthDto : ModelDto { };
    public class EducationDto : ModelDto { };

    public interface IGAUDFor<TModel> where TModel : IModelDto, new()
    {
        Result<TModel> GetByIdFor(int id);
        Result AddOrUpdateFor(TModel entity);
        Result DeleteFor<T>(int id) where T:TModel;
    }

    //public interface IRepository<T> : IGAUDFor<T> where T : IModelDto
    //{
        
    //}
    public abstract class GenericRepositoryWithDep<TModel, TDto> : GenericRepository<TModel, TDto>, 
    IGAUDFor<ModelDto> //where T : ModelDto, IModelDto, new()
    {
        public virtual Result AddOrUpdateFor(ModelDto entity)
        {
            throw new NotImplementedException();
        }

        public Result DeleteFor<T>(int id) where T : ModelDto
        {
            StackFrame fr = new StackFrame(0, true);
            StackTrace st = new StackTrace(fr);
            var ga = fr.GetMethod().GetGenericArguments();

            foreach (var item in ga)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(typeof(T).Name);
            }

            return Result.Ok();
        }

        public virtual Result<ModelDto> GetByIdFor(int id) 
        {
            throw new NotImplementedException();
        }
    }

    public class GenericRepository<TModel, TDto> : IGenericRepository<TModel, TDto>
    {
    }

    public interface IGenericRepository<TModel, TDto> 
    {
    }

    public class Person {}
    public class PersonDto {}

    public class PersonRepository : GenericRepositoryWithDep<Person, PersonDto>
    {
        public PersonRepository()
        {
        }
    }
}

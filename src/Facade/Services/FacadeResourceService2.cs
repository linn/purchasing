namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;

    using PagedList.Core;

    public abstract class FacadeResourceService2<T, TKey, TResource, TUpdateResource> : IFacadeResourceService2<T, TKey, TResource, TUpdateResource>
    {
        private readonly IRepository<T, TKey> repository;

        private readonly ITransactionManager transactionManager;

        private readonly IBuilder<T> resourceBuilder;

        protected FacadeResourceService2(
            IRepository<T, TKey> repository,
            ITransactionManager transactionManager,
            IBuilder<T> resourceBuilder)
        {
            this.repository = repository;
            this.transactionManager = transactionManager;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<TResource> GetById(TKey id, IEnumerable<string> privileges = null)
        {
            var entity = this.FindById(id);
            if (entity == null)
            {
                return new NotFoundResult<TResource>();
            }

            return new SuccessResult<TResource>(this.BuildResource(entity, privileges));
        }

        public IResult<IEnumerable<TResource>> GetAll(IEnumerable<string> privileges = null)
        {
            return new SuccessResult<IEnumerable<TResource>>(this.BuildResources(this.repository.FindAll(), privileges));
        }

        public IResult<IPagedList<TResource>> GetAll(int pageNumber, int pageSize, IEnumerable<string> privileges = null)
        {
            return new SuccessResult<IPagedList<TResource>>(
                new PagedList<TResource>(this.BuildResources(this.repository.FindAll(), privileges).AsQueryable(), pageNumber, pageSize));
        }

        public IResult<IPagedList<TResource>> GetAll<TKeySort>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TKeySort>> keySelectorForSort,
            bool asc,
            IEnumerable<string> privileges = null)
        {
            var result = asc ? this.repository.FindAll().OrderBy(keySelectorForSort) : this.repository.FindAll().OrderByDescending(keySelectorForSort);
            var resources = this.BuildResources(result, privileges).AsQueryable();
            return new SuccessResult<IPagedList<TResource>>(new PagedList<TResource>(resources, pageNumber, pageSize));
        }

        public IResult<IEnumerable<TResource>> Search(string searchTerm, IEnumerable<string> privileges = null)
        {
            try
            {
                return new SuccessResult<IEnumerable<TResource>>(
                    this.BuildResources(this.repository.FilterBy(this.SearchExpression(searchTerm)), privileges));
            }
            catch (NotImplementedException)
            {
                return new BadRequestResult<IEnumerable<TResource>>("Search is not implemented");
            }
        }

        public IResult<TResource> Add(TResource resource, IEnumerable<string> privileges = null, int? userNumber = null)
        {
            T entity;

            try
            {
                entity = this.CreateFromResource(resource);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<TResource>(exception.Message);
            }

            this.repository.Add(entity);
            this.MaybeSaveLog("Create", userNumber, entity, resource, default);

            this.transactionManager.Commit();

            return new CreatedResult<TResource>(this.BuildResource(entity, privileges));
        }

        public IResult<TResource> Update(TKey id, TUpdateResource updateResource, IEnumerable<string> privileges = null, int? userNumber = null)
        {
            var entity = this.FindById(id);
            if (entity == null)
            {
                return new NotFoundResult<TResource>();
            }

            try
            {
                this.UpdateFromResource(entity, updateResource);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<TResource>($"Error updating {id} - {exception.Message}");
            }

            this.MaybeSaveLog("Update", userNumber, entity, default, updateResource);
            this.transactionManager.Commit();

            return new SuccessResult<TResource>(this.BuildResource(entity, privileges));
        }

        public IResult<TResource> GetApplicationState(IEnumerable<string> privileges = null)
        {
            return new SuccessResult<TResource>(this.BuildResource(privileges));
        }

        public IResult<TResource> DeleteOrObsolete(TKey id, IEnumerable<string> privileges = null, int? userNumber = null)
        {
            var entity = this.FindById(id);
            if (entity == null)
            {
                return new NotFoundResult<TResource>();
            }

            this.DeleteOrObsoleteResource(entity);
            this.MaybeSaveLog("Delete", userNumber, entity, default, default);
            this.transactionManager.Commit();

            return new SuccessResult<TResource>(this.BuildResource(entity, privileges));
        }

        protected abstract T CreateFromResource(TResource resource);

        protected abstract void UpdateFromResource(T entity, TUpdateResource updateResource);

        protected abstract Expression<Func<T, bool>> SearchExpression(string searchTerm);

        protected abstract void DeleteOrObsoleteResource(T entity);

        protected abstract void SaveToLogTable(string actionType, int userNumber, T entity, TResource resource, TUpdateResource updateResource);

        protected void RemoveFromDatabase(T entity)
        {
            this.repository.Remove(entity);
        }

        protected TResource BuildResource(IEnumerable<string> privileges = null)
        {
            return this.BuildResource(default, privileges);
        }

        protected TResource BuildResource(T entity, IEnumerable<string> privileges = null)
        {
            return (TResource)this.resourceBuilder.Build(entity, privileges);
        }

        protected IEnumerable<TResource> BuildResources(IEnumerable<T> entities, IEnumerable<string> privileges = null)
        {
            return entities.Select(e => this.BuildResource(e, privileges));
        }

        private T FindById(TKey id)
        {
            return this.repository.FindById(id);
        }

        private void MaybeSaveLog(string actionType, int? userNumber, T entity, TResource resource, TUpdateResource updateResource)
        {
            if (!userNumber.HasValue)
            {
                return;
            }

            try
            {
                this.SaveToLogTable(actionType, userNumber.Value, entity, resource, updateResource);
            }
            catch (NotImplementedException)
            {
            }
        }
    }
}

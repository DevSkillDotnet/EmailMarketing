﻿using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public class FieldMapService : IFieldMapService
    {
        private IFieldMapUnitOfWork _fieldMapUnitOfWork;
        public FieldMapService(IFieldMapUnitOfWork fieldMapUnitOfWork)
        {
            _fieldMapUnitOfWork = fieldMapUnitOfWork;
        }

        public async Task AddAsync(FieldMap entity)
        {
            var isExists = await _fieldMapUnitOfWork.FieldMapRepository.IsExistsAsync(x => x.DisplayName == entity.DisplayName && x.Id != entity.Id);
            if (isExists)
                throw new DuplicationException(nameof(entity.DisplayName));

            await _fieldMapUnitOfWork.FieldMapRepository.AddAsync(entity);
            await _fieldMapUnitOfWork.SaveChangesAsync();
        }

        public async Task<FieldMap> DeleteAsync(int id)
        {
            var fieldMap = await GetByIdAsync(id);
            if (fieldMap == null) throw new NotFoundException(nameof(FieldMap), id);
            await _fieldMapUnitOfWork.FieldMapRepository.DeleteAsync(id);
            await _fieldMapUnitOfWork.SaveChangesAsync();
            return fieldMap;
        }

        public async Task<(IList<FieldMap> Items, int Total, int TotalFilter)> GetAllAsync(
            Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<FieldMap, object>>>()
            {
                ["DisplayName"] = v => v.DisplayName
            };
            var result = await _fieldMapUnitOfWork.FieldMapRepository.GetAsync<FieldMap>(
                x => x, x => !x.IsStandard && (!userId.HasValue || x.UserId == userId.Value) && (string.IsNullOrWhiteSpace(searchText) || x.DisplayName.Contains(searchText)),
                x => x.ApplyOrdering(columnsMap, orderBy), null,
                pageIndex, pageSize, true);
            return (result.Items, result.Total, result.TotalFilter);
        }

        public async Task<FieldMap> GetByIdAsync(int id)
        {
            var result = await _fieldMapUnitOfWork.FieldMapRepository.GetByIdAsync(id);
            if (result == null) throw new NotFoundException(nameof(FieldMap), id);
            return result;
        }

        public async Task UpdateAsync(FieldMap entity)
        {
            var isExists = await _fieldMapUnitOfWork.FieldMapRepository.IsExistsAsync(x => x.DisplayName == entity.DisplayName && x.Id != entity.Id);
            if (isExists)
                throw new DuplicationException(nameof(entity.DisplayName));

            var updateEntity = await GetByIdAsync(entity.Id);
            updateEntity.DisplayName = entity.DisplayName;

            await _fieldMapUnitOfWork.FieldMapRepository.UpdateAsync(updateEntity);
            await _fieldMapUnitOfWork.SaveChangesAsync();
        }
        public void Dispose()
        {
            _fieldMapUnitOfWork?.Dispose();
        }
    }
}

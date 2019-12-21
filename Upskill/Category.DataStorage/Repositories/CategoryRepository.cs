using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Category.DataStorage.Dtos;
using Microsoft.EntityFrameworkCore;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Category.DataStorage.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDbContext _context;

        public CategoryRepository(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<IDataResult<CategoryDto>> GetById(string id)
        {
            return await GetBy(x => x.Id == id);
        }

        public async Task<IDataResult<CategoryDto>> GetByName(string name)
        {
            return await GetBy(x => x.Name == name);
        }

        public async Task<IReadOnlyCollection<CategoryDto>> GetAll()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return categories.Select(this.MapToCategoryDto).ToList();
        }

        public async Task<IResult> Create(CategoryDto categoryDto)
        {
            var category = this.MapFromCategoryDto(categoryDto);

            _context.Categories.Add(category);
            return await this.SaveChanges();
        }

        public async Task<IResult> Update(CategoryDto categoryDto)
        {
            var category = this.MapFromCategoryDto(categoryDto);

            _context.Entry(category).State = EntityState.Modified;
            return await this.SaveChanges();
        }

        public async Task<IResult> Delete(string id)
        {
            var employer = new Models.Category { Id = id };
            _context.Entry(employer).State = EntityState.Deleted;

            return await this.SaveChanges();
        }

        private async Task<IResult> SaveChanges()
        {
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0 ? (IResult)new SuccessfulResult() : new FailedResult();
        }

        private CategoryDto MapToCategoryDto(Models.Category category)
        {
            return new CategoryDto(category.Id, category.Name, category.Description, category.SortOrder);
        }

        private Models.Category MapFromCategoryDto(CategoryDto category)
        {
            return new Models.Category(category.Id, category.Name, category.Description, category.SortOrder);
        }

        private async Task<IDataResult<CategoryDto>> GetBy(Expression<Func<Models.Category, bool>> by)
        {
            var category = await _context.Categories.Where(by).AsNoTracking().FirstOrDefaultAsync(by);

            if (category == null)
            {
                return new FailedDataResult<CategoryDto>();
            }

            return new SuccessfulDataResult<CategoryDto>(this.MapToCategoryDto(category));
        }
    }
}
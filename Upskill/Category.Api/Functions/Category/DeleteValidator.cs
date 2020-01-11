using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Results;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteValidator
    {
        private readonly IDeleteValidator _deleteValidator;

        public DeleteValidator(IDeleteValidator deleteValidator)
        {
            _deleteValidator = deleteValidator;
        }


        [FunctionName(nameof(DeleteValidator))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "category/delete/{id:guid}")] HttpRequest req,
            string id)
        {
            var canBeDeleted = await _deleteValidator.CanDelete(id);

            if (!canBeDeleted.Success)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
    }
}

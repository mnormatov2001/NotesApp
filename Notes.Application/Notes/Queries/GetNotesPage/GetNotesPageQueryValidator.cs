using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotesPage
{
    public class GetNotesPageQueryValidator : AbstractValidator<GetNotesPageQuery>
    {
        public GetNotesPageQueryValidator()
        {
            RuleFor(query => query.UserId).NotEqual(Guid.Empty);
            RuleFor(query => query.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(query => query.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}

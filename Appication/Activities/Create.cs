using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Appication.Activities {
    public class Create {
        public class Command : IRequest {
            // i love my girlfriend
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }

        }

/*

need to define the HTTP errors. need to find where these error woll be placed 

*/


        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Venue).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command> {
            private readonly DataContext _context;
            public Handler (DataContext context) {
                this._context = context;

            }

            public async Task<Unit> Handle (Command request,
                CancellationToken cancellationToken) {

                // add anew activity
                var activity = new Activity {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Category = request.Category,
                    Date = request.Date,
                    City = request.City,
                    Venue = request.Venue
                };

                _context.Activities.Add (activity);
                var success = await _context.SaveChangesAsync () > 0;

                if (success) return Unit.Value; // if the value was added to the database

                throw new Exception ("problem saving changes");

            }
        }

    }
}
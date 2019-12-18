using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Appication.Errors;
using MediatR;
using Persistence;

namespace Appication.Activities {
    public class Delete {
        public class Command : IRequest {
            // i love my girlfriend

            public Guid Id { get; set; }

        }

        public class Handler : IRequestHandler<Command> {
            private readonly DataContext _context;
            public Handler (DataContext context) {
                this._context = context;

            }

            // handler logic 
            public async Task<Unit> Handle (Command request,
                CancellationToken cancellationToken) {

                var activity = await _context.Activities.FindAsync (request.Id);

                if (activity == null) 
                    throw new RestException(HttpStatusCode.NotFound, new {activity = "Not Found"});

                _context.Remove (activity);

                var success = await _context.SaveChangesAsync () > 0;

                if (success) return Unit.Value; // if the value was added to the database

                throw new Exception ("problem saving changes");

            }
        }
    }
}
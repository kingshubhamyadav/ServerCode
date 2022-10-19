using OnDemandCarWash.Dtos;
using OnDemandCarWash.Repositories;

namespace OnDemandCarWash.Services
{
    public class EmailService
    {
        private readonly IEmailRepository _repo;
        public EmailService(IEmailRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public string SendEmail(EmailDto request)
        {
            return _repo.SendEmail(request);
        }
    }
}

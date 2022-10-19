using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;

namespace OnDemandCarWash.Repositories
{
    public interface IEmailRepository
    {
        string SendEmail(EmailDto request);
    }
}

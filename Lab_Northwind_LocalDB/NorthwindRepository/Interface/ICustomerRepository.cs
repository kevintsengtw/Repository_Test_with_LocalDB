using System.Collections.Generic;
using NorthwindRepository.Models;

namespace NorthwindRepository.Interface
{
    public interface ICustomerRepository
    {
        List<CustomerModel> GetAll();

        CustomerModel Get(string customerId);
    }
}
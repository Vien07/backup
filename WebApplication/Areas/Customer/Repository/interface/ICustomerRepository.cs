using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Customer.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Customer
{
    public interface ICustomerRepository
    {
        dynamic LoadData(SearchDto search);
        dynamic Edit(int Pid);
        dynamic Insert(Models.Customer customer, IFormFile avatar);
        dynamic Update(Models.Customer customer, IFormFile avatar);
        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic LoadCustomer(int Pid);
        dynamic LoadOrders(int customerPid);
    }
}
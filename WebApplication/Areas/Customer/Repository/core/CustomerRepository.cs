using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Customer.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using DTO.Cart;

namespace CMS.Areas.Customer
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;

        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;

        public CustomerRepository(DBContext dbContext,
                             IFileServices fileHelper, ICommonServices common)
        {
            _dbContext = dbContext;
            _fileServices = fileHelper;
            _common = common;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                var data = (from a in _dbContext.Customers
                            where a.Deleted == false && (a.Enabled == search.Enable || search.Enable == null)
                            select new
                            {
                                Email = a.Email,
                                Pid = a.Pid,
                                PhoneNumber = a.PhoneNumber,
                                Note = a.Note,
                                FullName = a.FirstName + " " + a.LastName,
                                //FullName = a.FullName,
                                PicThumb = UrlCustomerImages + a.PicThumb,
                                Enabled = a.Enabled,
                                CreateDate = a.CreateDate,
                                Province = a.Province,
                                District = a.District,
                                Ward = a.Ward,
                                Address = a.Address,
                                StringAddress = _common.GetAddress(a.Address, _common.GetWard(a.District, a.Ward), _common.GetDistrict(a.Province, a.District), _common.GetProvince(a.Province))
                            }).ToList().FilterSearch(new string[] { "Email", "PhoneNumber", "FullName" }, search.Key);

                PagedList<dynamic> dataPaging = new PagedList<dynamic>(data.OrderByDescending(p => p.CreateDate).ToList(), search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);

                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { Data = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Enable(long[] Pid, bool active)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Customers.Where(p => p.Pid == item).FirstOrDefault();
                        model.Enabled = active;
                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public dynamic Delete(int Pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = _dbContext.Customers.FirstOrDefault(p => p.Pid == Pid);

                    var customerOrderList = _dbContext.Orders.Where(x => x.CustomerPid == model.Pid).ToList();
                    if (customerOrderList.Any())
                    {
                        return new { isError = false, messError = "Khách hàng đã có đơn hàng!" };
                    }

                    if(model.PicThumb != "default-avatar.png")
                    {
                        _fileServices.DeleteFile(UrlCustomerImages, model.PicThumb);
                    }
                    _dbContext.Customers.Remove(model);
                    _dbContext.SaveChanges();

                    dynamic logObj = new ExpandoObject();
                    logObj.Title = model.Email;
                    logObj.Pid = model.Pid;
                    logObj.Cate = ConstantStrings.CustomerId;
                    _common.SaveLog(1, "delete", logObj);

                    transaction.Commit();
                    return new { isError = true, messError = "" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new { isError = false, messError = "Something is wrong!" };
                }
            }
        }

        public dynamic Delete(long[] Pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Check if any customer has associated orders
                    var customerOrderList = _dbContext.Orders.Where(x => Pid.Contains(x.CustomerPid)).ToList();
                    if (customerOrderList.Any())
                    {
                        return new { isError = false, messError = "Khách hàng đã có đơn hàng!" };
                    }

                    // Proceed with deleting customers
                    foreach (var item in Pid)
                    {
                        try
                        {
                            var model = _dbContext.Customers.FirstOrDefault(p => p.Pid == item);
                            if (model != null)
                            {
                                if (model.PicThumb != "default-avatar.png")
                                {
                                    _fileServices.DeleteFile(UrlCustomerImages, model.PicThumb);
                                }
                                _dbContext.Customers.Remove(model);
                                _dbContext.SaveChanges();

                                dynamic logObj = new ExpandoObject();
                                logObj.Title = model.Email;
                                logObj.Pid = model.Pid;
                                logObj.Cate = ConstantStrings.CustomerId;
                                _common.SaveLog(1, "delete", logObj);
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new { isError = false, messError = "Something is wrong!" };
                        }
                    }

                    transaction.Commit();
                    return new { isError = true, messError = "" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new { isError = false, messError = "Something is wrong!" };
                }
            }
        }

        public dynamic Edit(int Pid)
        {
            try
            {
                var customer = _dbContext.Customers.Where(p => p.Pid == Pid).FirstOrDefault();
                customer.PicThumb = UrlCustomerImages + customer.PicThumb;
                return customer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public dynamic Insert(Models.Customer customer, IFormFile avatar)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";
                try
                {
                    var accountExists = _dbContext.Customers.Where(x => x.Email.Equals(customer.Email) && x.Deleted == false).FirstOrDefault();
                    if (accountExists == null)
                    {
                        customer.Password = _common.GetHashSha256(customer.Password);
                        if (avatar != null)
                        {
                            var img = _fileServices.SaveFileAvatar(avatar, UrlCustomerImages, customer.FullName);
                            if (!img.isError)
                            {
                                if (customer.PicThumb != "default-avatar.png")
                                {
                                    _fileServices.DeleteFile(UrlCustomerImages, customer.PicThumb);
                                }
                                customer.PicThumb = img.fileName;
                            }
                        }
                        else
                        {
                            customer.PicThumb = "default-avatar.png";
                        }
                        _dbContext.Customers.Add(customer);
                        _dbContext.SaveChanges();

                        #region save log
                        dynamic logObj = new ExpandoObject();
                        logObj.Title = customer.Email;
                        logObj.Pid = customer.Pid;
                        logObj.Cate = ConstantStrings.CustomerId;
                        _common.SaveLog(1, "new", logObj);
                        #endregion
                        transaction.Commit();
                        return new { status = true, mess = messErr };
                    }
                    messErr = "Email đã tồn tại trong hệ thống!";
                    return new { status = false, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";
                    return new { status = false, mess = messErr };
                }
            }
        }
        public dynamic Update(Models.Customer customer, IFormFile avatar)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";
                try
                {
                    var model = _dbContext.Customers.Where(x => !x.Deleted && x.Pid == customer.Pid).FirstOrDefault();
                    if (!string.IsNullOrEmpty(customer.Password))
                    {
                        model.Password = _common.GetHashSha256(customer.Password);
                    }

                    if (avatar != null)
                    {
                        var img = _fileServices.SaveFileAvatar(avatar, UrlCustomerImages, customer.FullName);
                        if (!img.isError)
                        {
                            if (model.PicThumb != "default-avatar.png")
                            {
                                _fileServices.DeleteFile(UrlCustomerImages, model.PicThumb);
                            }
                            model.PicThumb = img.fileName;
                        }
                    }

                    model.FirstName = customer.FirstName;
                    model.LastName = customer.LastName;
                    model.FullName = customer.FullName;
                    model.Note = customer.Note;
                    model.Enabled = customer.Enabled;
                    model.Province = customer.Province;
                    model.District = customer.District;
                    model.Ward = customer.Ward;
                    model.PhoneNumber = customer.PhoneNumber;
                    model.Address = customer.Address;
                    model.Zalo = customer.Zalo;
                    model.Facebook = customer.Facebook;
                    model.Twitter = customer.Twitter;
                    model.LinkedIn = customer.LinkedIn;
                    model.Website = customer.Website;
                    _dbContext.SaveChanges();

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = customer.Email;
                    logObj.Pid = customer.Pid;
                    logObj.Cate = ConstantStrings.CustomerId;
                    _common.SaveLog(1, "update", logObj);
                    #endregion
                    transaction.Commit();
                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";
                    return new { status = false, mess = messErr };
                }
            }
        }
        public dynamic LoadCustomer(int Pid)
        {
            try
            {
                var customer = _dbContext.Customers.Where(p => p.Pid == Pid).FirstOrDefault();
                customer.PicThumb = UrlCustomerImages + customer.PicThumb;
                return customer;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public dynamic LoadOrders(int customerPid)
        {
            try
            {
                var enumStateOrderList = new Dictionary<int, string>();
                var stateOrderList = Enum.GetNames(typeof(EnumOrder.OrderState));
                for (var i = 0; i < stateOrderList.Length; i++)
                {
                    enumStateOrderList.Add(i, stateOrderList[i]);
                }

                var enumPaymentMethodList = new Dictionary<int, string>();
                var paymentMethodList = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                for (var i = 0; i < paymentMethodList.Length; i++)
                {
                    enumPaymentMethodList.Add(i, paymentMethodList[i]);
                }

                var data = (from a in _dbContext.Customers
                            join b in _dbContext.Orders on a.Pid equals b.CustomerPid
                            where b.Deleted == false && b.CustomerPid == customerPid
                            select new
                            {
                                Pid = b.Pid,
                                Status = enumStateOrderList.GetValueOrDefault(b.State),
                                StatusId = b.State,
                                IsPaymentId = b.IsPayment,
                                PaymentMethodId = b.PaymentMethod,
                                IsPayment = b.IsPayment ? "Đã thanh toán" : "Chưa thanh toán",
                                PaymentMethod = enumPaymentMethodList.GetValueOrDefault(b.PaymentMethod),
                                Note = b.Note,
                                TotalString = _common.ConvertFormatMoney(b.Total),
                                ShipFeeString = _common.ConvertFormatMoney(b.ShipFee),
                                DepositString = _common.ConvertFormatMoney(b.Deposit),
                                Enabled = b.Enabled,
                                //Customer = _dbContext.Customers.Where(x => x.Pid == b.CustomerPid).FirstOrDefault(),
                                CreateDate = b.CreateDate,
                                CreateUser = b.CreateUser,
                                UpdateUser = b.UpdateUser,
                            }).OrderByDescending(p => p.CreateDate).ToList();
                return Newtonsoft.Json.JsonConvert.SerializeObject(data); ;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
    }
}

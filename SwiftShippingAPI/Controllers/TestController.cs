﻿using AutoMapper;
using E_CommerceAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftShipping.DataAccessLayer.Enum;
using SwiftShipping.DataAccessLayer.Models;
using SwiftShipping.DataAccessLayer.Repository;
using SwiftShipping.ServiceLayer.DTO;
using SwiftShipping.ServiceLayer.Services;

namespace SwiftShipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        SellerService sellerService;
        DeliveryManService deliveryManService;
        OrderService orderService;
        GovernmentService governmentService;
        RegionService regionService;
        AdminService adminService;

        private readonly IMapper _mapper;
        private readonly UnitOfWork unit;
        public TestController(SellerService _sellerService, DeliveryManService _deliveryManService, OrderService _orderService
            , GovernmentService _governmentService, RegionService _regionService,
            IMapper mapper, UnitOfWork unit, AdminService adminService)
        {

            sellerService = _sellerService;
            deliveryManService = _deliveryManService;
            orderService = _orderService;
            this.governmentService = _governmentService;
            regionService = _regionService;
            _mapper = mapper;
            this.unit = unit;
            this.adminService = adminService;
        }

        [HttpPost("addSeller")]
        public async Task<IActionResult> addSeller(SellerDTO sellerDTO)
        {

            if (ModelState.IsValid)
            {
                await sellerService.addSellerAsync(sellerDTO);

                return Ok("seller Added Successfully");


            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addDeliveryMan")]
        public async Task<IActionResult> addDeliveryMan(DeliveryManDTO deliveryMan)
        {

            if (ModelState.IsValid)
            {
                await deliveryManService.AddDliveryManAsync(deliveryMan);

                return Ok("delivery Man Added Successfully");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> addOrder(OrderDTO orderData)
        {

            if (ModelState.IsValid)
            {

                orderService.AddOrder(orderData);
                return Ok("Order Added Successfully");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addGovernment")]
        public async Task<IActionResult> addGovernment(string name)
        {

            if (ModelState.IsValid)
            {

                governmentService.AddGovernment(name);
                return Ok("Government Added Successfully");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addRegion")]
        public async Task<IActionResult> addRegion(RegionDTO regionDTO)
        {

            if (ModelState.IsValid)
            {

                regionService.Add(regionDTO);
                return Ok("Region Added Successfully");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("testGetById")]

        public IActionResult TestGetById()
        {
            //var deliveryRegions = unit.DeliveryManRegionsRipository.GetById(5, 1);
            var user = unit.AppUserRepository.GetById("b8e197ce-0bc5-4aaf-9212-e42ef29b6bc8");
            return Ok(new { Name = user.Name });
        }

        [HttpGet("auth")]
        [Authorize(Roles = "Employee")]
        //[Authorize(Policy = "Employees/View")]
        public ActionResult TestAuth()
        {
            return Ok("You Are Authorized");
        }

        [HttpGet("enum")]
        public IActionResult TestEnum()
        {
            return Ok(RoleTypes.GetNames(typeof(RoleTypes)).ToList());
        }
        [HttpGet("seedPermissions")]
        public void SeedRolePermissions()
        {
            var roles = RoleTypes.GetNames(typeof(RoleTypes)).ToList();

            var departments = Department.GetValues(typeof(Department)).Cast<Department>().ToList();

            foreach (var role in roles)
            {
                foreach (var department in departments)
                {
                    unit.RolePermissionsRepository.Insert(new RolePermissions
                    {
                        RoleName = role,
                        DepartmentId = department,
                        View = true,
                        Edit = false,
                        Delete = false,
                        Add = false
                    });
                    unit.SaveChanges();
                }
            }
        }
        [HttpPost("addAdmin")]
        public async Task<IActionResult> AddAdmin()
        {

            if (ModelState.IsValid)
            {
                await adminService.addAdminAsync();
                return Ok();
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }
        }

    }

}

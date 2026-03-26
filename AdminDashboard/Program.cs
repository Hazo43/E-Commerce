using AutoMapper;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Presistence.Data.Dbcontexts;
using Presistence.Identity.DbContext;
using StackExchange.Redis;
using Stripe;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Principal;

namespace AdminDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // StoreDbContext
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // IdentityDbContext
            builder.Services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            // User Identity Role 
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityStoreDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
//        1- Project setup
//-- add project mvc named adminDashboard in api project solution
//-- install dashboard template
//-- add project reference from admindashboard to talabat.APIs
//-- copy connection string from api and add it in admindashboard without redis
//-- in talabat api in the startup copy the part of configure db contexts and add them in mvc project 
//-- in talabat api copy the part of identity from identityserviceextension and add it in the startup
//-- in admindashboard in layout add the links for css , js..and add them in wwwroot
//-- add the nav bar in the layout and remove the search
//-- add the side bar in the layout, remove everything inside layoutSidenav_content div and add the div container inside it
//-- remove the footer and uncessary things
//-- in sidebar : 
//-- add new div Secuirty and make asp-controller = Role , asp-action = index , change icon to roles and dashboard word to roles
//-- add new link Users and asp-controller = user and asp-action = Index
//*****************************************************************************************************************
                          //2- ManageRoles 
//-- add RoleController, inject RoleManager
//-- implement index in role controller
//-- add view to index 
//-- add new vm RoleFormViewModel
//-- add RoleFormPartialView in views
//-- render the partial view in the index page 
//-- in controller add the create action , delete action
//-- add new vm RoleViewModel
//-- in controller add the edit action 
//-- add view for edit 
//-- in controller add the edit post action 
//*****************************************************************************************************************
                                 //3- Manage users
//-- add user controller
//-- create userViewModel
//-- create index action
//-- create index view
//-- add UserRoleViewModel
//-- add edit action get
//-- add edit view
//-- add edit action post
//*****************************************************************************************************************
                               //4- Login/Logout
//-- add controller AdminController
//-- add login action get  in controller
//-- add login view
//-- edit code of template and add aspaction in the method
//-- edit route in startup
//-- add login action post in controller
//-- add logout action in controller
//-- add asp-action and asp-controller for logout in the layout 
//*****************************************************************************************************************
                              //5- Manage product 
//-- in layout add another part for Products and brands
//-- add new controller ProductController
//-- add ProductViewModel
//-- add new folder Helpers
//-- add new class in helpers , MapsProfile include mapps profile
//-- in startup, allow dependency injection for Iunit of work , Maps profile
//-- add the index action to get all products
//-- add the view that include all the products
//-- copy images folder from wwwroot in api project to my project
//-- in helpers add pictureSetting class
//-- add brandConfig in Talabat.Repository
//-- add migration the default pro is talabat.repository 
//-- in product controller add create action[get]
//-- add partialview called CreatEditPartialView, inject in it IUnitOfWork, Brands, Types
//-- add create product view 
//-- add create action[post]
//-- add edit action in the productcontroller[get]
//-- change create view to render partial view
//-- add edit view
//-- add edit action[post]
//-- add delete action[get]
//-- add delete view
//-- add delete action[post]
//*****************************************************************************************************************
                              //6- Manage Brands
//-- add BrandsController
//-- add index action 
//-- add index view
//-- add create action 
//-- add CreateFormPartialView 
//-- add delete action

    }
}

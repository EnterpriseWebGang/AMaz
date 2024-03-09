using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMaz.DB;
using AutoMapper;
using System.Security.Cryptography;
using AutoMapper.Execution;
using AMaz.Repo;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Service
{
    public class AdminService : IAdminService
    {
     //   private readonly AMazDbContext _db;
        private readonly IAdminResponsitory _adminRepo;
        private readonly IMapper _mapper;
        public AdminService (
            IAdminResponsitory adminRepo,
            
            IMapper mapper)
        {
            _adminRepo = adminRepo; 
            _mapper = mapper;
        }
        public bool AdminCheck()
        {
            return  _adminRepo.IsUserExistsAsync("admin@gmail.com");
          //  return _db.Users.Any(_=>_.Email == "admin@gmail.com");
        }

        public AuthenticateResponse Create(CreateRequest model)
        {
            if ( _adminRepo.IsUserExistsAsync(model.Email))
                throw new AppException($"Email '{model.Email}'is already registered");
            // map model to new account object
            var account = _mapper.Map<User>(model);

            //hash password
            account.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            //save account

            // await _adminRepo.AddUserAsync(account);
            //await _adminRepo.SaveChangesAsync();

            //_db.Users.Add(account);

            //_db.SaveChanges();
            _adminRepo.AddUser(account);
            _adminRepo.SaveChanges();

            return _mapper.Map<AuthenticateResponse>(account);
        }
    }
}

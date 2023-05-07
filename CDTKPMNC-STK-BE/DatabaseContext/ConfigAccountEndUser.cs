﻿using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DatabaseContext
{
    namespace P02_FluentApi
    {
        public class ConfigAccountEndUser : IEntityTypeConfiguration<AccountEndUser>
        {
            public void Configure(EntityTypeBuilder<AccountEndUser> builder)
            {
                builder.HasIndex(ua => ua.UserName)
                        .IsUnique();
            }
        }
    }
}
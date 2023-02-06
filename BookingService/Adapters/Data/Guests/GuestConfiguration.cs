using Domain.Guests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Guests
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest> // For add 2 more value colums on a new property of guest object to guest database table, we need to declare this configuration for EF Core 
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasKey(x => x.id);
            builder.OwnsOne(x => x.Document)
                   .Property(x => x.IdNumber);

            builder.OwnsOne(x => x.Document)
                   .Property(x => x.DocumentType);
        }
    }
}

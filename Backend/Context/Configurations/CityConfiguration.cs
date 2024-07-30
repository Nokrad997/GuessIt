using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        //tableConfig
        builder.ToTable("cities");
        builder.HasKey(c => c.CityId)
            .HasName("PK_cities");
        
        //properties
        builder.Property(c => c.CityId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("city_id");
        builder.Property(c => c.CityName)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("city_name");
        builder.Property(c => c.CountryIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("country_id");
        builder.Property(c => c.GeolocationIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("geolocation_id");
        
        //relations
        builder.HasOne(c => c.Country)
            .WithMany(co => co.Cities)
            .HasForeignKey(c => c.CountryIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_cities_countries");
        builder.HasOne(c => c.Geolocation)
            .WithMany(g => g.Cities)
            .HasForeignKey(c => c.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_cities_geolocations");
    }
}
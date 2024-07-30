using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        //tableConfig
        builder.ToTable("countries");
        builder.HasKey(c => c.CountryId)
            .HasName("PK_countries");
        
        //properties
        builder.Property(c => c.CountryId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("country_id");
        builder.Property(c => c.CountryName)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("country_name");
        builder.Property(c => c.ContinentIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("continent_id");
        builder.Property(c => c.GeolocationIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("geolocation_id");
        
        //relationships
        builder.HasOne(c => c.Continent)
            .WithMany(co => co.Countries)
            .HasForeignKey(c => c.ContinentIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_countries_continents");
        builder.HasOne(c => c.Geolocation)
            .WithMany(g => g.Countries)
            .HasForeignKey(c => c.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_countries_geolocations");
    }
}
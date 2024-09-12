using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class GeolocationConfiguration : IEntityTypeConfiguration<Geolocation>
{
    public void Configure(EntityTypeBuilder<Geolocation> builder)
    {
        //tableConfig
        builder.ToTable("geolocations");
        builder.HasKey(g => g.GeolocationId)
            .HasName("PK_Geolocations");
        
        //properties
        builder.Property(g => g.GeolocationId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("geolocation_id");
        builder.Property(g => g.Area)
            .IsRequired()
            .HasColumnType("geometry(MultiPolygon)")
            .HasColumnName("area");
        
        //relations
        builder.HasMany(g => g.Cities)
            .WithOne(c => c.Geolocation)
            .HasForeignKey(c => c.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Cities_Geolocations");
        builder.HasMany(g => g.Countries)
            .WithOne(co => co.Geolocation)
            .HasForeignKey(co => co.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Countries_Geolocations");
        builder.HasMany(g => g.Continents)
            .WithOne(con => con.Geolocation)
            .HasForeignKey(con => con.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Continents_Geolocations");
    }
}
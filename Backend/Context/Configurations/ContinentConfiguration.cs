using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class ContinentConfiguration : IEntityTypeConfiguration<Continent>
{
    public void Configure(EntityTypeBuilder<Continent> builder)
    {
        //tableConfig
        builder.ToTable("continents");
        builder.HasKey(c => c.ContinentId)
            .HasName("PK_continents");
        
        //properties
        builder.Property(c => c.ContinentId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("continent_id");
        builder.Property(c => c.ContinentName)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("continent_name");
        builder.Property(c => c.GeolocationIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("geolocation_id");
        
        //relations
        builder.HasOne(c => c.Geolocation)
            .WithMany(g => g.Continents)
            .HasForeignKey(c => c.GeolocationIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_continents_geolocations");
    }
}
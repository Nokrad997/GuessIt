from sqlalchemy import create_engine, Column, Integer, String, ForeignKey
from sqlalchemy.orm import sessionmaker, relationship, declarative_base
from geoalchemy2 import Geometry
from geojson import load as load_geojson
from sqlalchemy import func
import os
import json

# Database connection settings
DATABASE_URL = "postgresql://postgres:root@localhost:5432/GuessIt"
engine = create_engine(DATABASE_URL)
Session = sessionmaker(bind=engine)
session = Session()
Base = declarative_base()

# Geolocation model
class Geolocation(Base):
    __tablename__ = 'geolocations'
    geolocation_id = Column(Integer, primary_key=True)
    area = Column(Geometry(geometry_type='MULTIPOLYGON', srid=4326))

    # Relationships
    continents = relationship("Continent", back_populates="geolocation")
    countries = relationship("Country", back_populates="geolocation")

# Continent model
class Continent(Base):
    __tablename__ = 'continents'
    continent_id = Column(Integer, primary_key=True)
    continent_name = Column(String)
    geolocation_id = Column(Integer, ForeignKey('geolocations.geolocation_id'))
    geolocation = relationship("Geolocation", back_populates="continents")
    countries = relationship("Country", back_populates="continent")

# Country model
class Country(Base):
    __tablename__ = 'countries'
    country_id = Column(Integer, primary_key=True)
    country_name = Column(String)
    geolocation_id = Column(Integer, ForeignKey('geolocations.geolocation_id'))
    continent_id = Column(Integer, ForeignKey('continents.continent_id'))
    geolocation = relationship("Geolocation", back_populates="countries")
    continent = relationship("Continent", back_populates="countries")

# Converts coordinates to WKT format
def coordinates_to_wkt(coordinates, geometry_type):
    if geometry_type == 'Polygon':
        rings = [f"({', '.join([f'{point[0]} {point[1]}' for point in ring])})" for ring in coordinates]
        return f"POLYGON({', '.join(rings)})"
    elif geometry_type == 'MultiPolygon':
        polygons = []
        for polygon in coordinates:
            rings = [f"({', '.join([f'{point[0]} {point[1]}' for point in ring])})" for ring in polygon]
            polygons.append(f"({', '.join(rings)})")
        return f"MULTIPOLYGON({', '.join(polygons)})"
    else:
        raise ValueError(f"Unsupported geometry type: {geometry_type}")

# Create geolocation and validate geometry
def create_geolocation(geometry_type, coordinates):
    wkt_geometry = coordinates_to_wkt(coordinates, geometry_type)
    valid_geometry_query = func.ST_MakeValid(func.ST_SetSRID(func.ST_GeomFromEWKT(f'SRID=4326;{wkt_geometry}'), 4326))
    is_valid = session.query(func.ST_IsValid(valid_geometry_query)).scalar()

    if not is_valid:
        print(f"Warning: Geometry was invalid, fixing geometry.")
        wkt_geometry = session.query(valid_geometry_query).scalar()

    geolocation = Geolocation(area=wkt_geometry)
    session.add(geolocation)
    session.commit()
    return geolocation.geolocation_id

# Process a single continent and its countries
def process_continent_and_countries(base_path, continent):
    continent_code = continent['continent_code']
    continent_name = continent['continent_name']
    continent_file_path = os.path.join(base_path, 'continents', f"{continent_code}.json")

    # Read continent GeoJSON
    with open(continent_file_path, 'r', encoding='utf-8') as continent_file:
        continent_geojson = load_geojson(continent_file)

    # Create geolocation for the continent
    geolocation_id = create_geolocation(continent_geojson['geometry']['type'], continent_geojson['geometry']['coordinates'])

    # Add continent to database
    new_continent = Continent(
        continent_name=continent_name,
        geolocation_id=geolocation_id
    )
    session.add(new_continent)
    session.commit()

    # Process countries for this continent
    with open(os.path.join(base_path, 'countries', 'countries.json'), 'r', encoding='utf-8') as file:
        countries_data = json.load(file)

    for country in countries_data:
        if country['country_a2'] in continent['countries']:
            country_code = country['country_a2']
            country_file_path = os.path.join(base_path, 'countries', f"{country_code}.json")
            
            # Skip if the country file does not exist
            if not os.path.exists(country_file_path):
                print(f"Warning: No GeoJSON file found for country code: {country_code}")
                continue

            with open(country_file_path, 'r', encoding='utf-8') as country_file:
                country_geojson = load_geojson(country_file)

            # Create geolocation for the country
            country_geolocation_id = create_geolocation(country_geojson['geometry']['type'], country_geojson['geometry']['coordinates'])

            # Add country to database
            new_country = Country(
                country_name=country['country_name'],
                geolocation_id=country_geolocation_id,
                continent_id=new_continent.continent_id
            )
            session.add(new_country)
    session.commit()

# Process all continents and their countries
def process_all_continents_and_countries(base_path):
    with open(os.path.join(base_path, 'continents', 'continents.json'), 'r', encoding='utf-8') as file:
        continents_data = json.load(file)

    for continent in continents_data:
        process_continent_and_countries(base_path, continent)

# Main execution
base_path = 'GeolocationsGeoJSONs'
process_all_continents_and_countries(base_path)

print("Data has been processed and stored in the database.")

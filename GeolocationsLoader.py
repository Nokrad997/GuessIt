from sqlalchemy import create_engine, Column, Integer, String, ForeignKey
from sqlalchemy.orm import sessionmaker, relationship, declarative_base
from geoalchemy2 import Geometry
from geojson import load as load_geojson
from sqlalchemy import func

# Ustawienia połączenia z bazą danych PostgreSQL z PostGIS
DATABASE_URL = "postgresql://postgres:root@localhost:5432/GuessIt"
engine = create_engine(DATABASE_URL)
Session = sessionmaker(bind=engine)
session = Session()
Base = declarative_base()

# Definicja modelu Geolocation
class Geolocation(Base):
    __tablename__ = 'geolocations'
    geolocation_id = Column(Integer, primary_key=True)
    area = Column(Geometry(geometry_type='MULTIPOLYGON', srid=4326))  # Użyj MULTIPOLYGON

    # Relacje z miastami, krajami i kontynentami
    cities = relationship("City", back_populates="geolocation")
    countries = relationship("Country", back_populates="geolocation")
    continents = relationship("Continent", back_populates="geolocation")

# Definicja modelu Continent
class Continent(Base):
    __tablename__ = 'continents'
    continent_id = Column(Integer, primary_key=True)
    continent_name = Column(String)
    geolocation_id = Column(Integer, ForeignKey('geolocations.geolocation_id'))
    geolocation = relationship("Geolocation", back_populates="continents")
    countries = relationship("Country", back_populates="continent")

# Definicja modelu Country
class Country(Base):
    __tablename__ = 'countries'
    country_id = Column(Integer, primary_key=True)
    country_name = Column(String)
    continent_id = Column(Integer, ForeignKey('continents.continent_id'))
    geolocation_id = Column(Integer, ForeignKey('geolocations.geolocation_id'))
    geolocation = relationship("Geolocation", back_populates="countries")
    continent = relationship("Continent", back_populates="countries")
    cities = relationship("City", back_populates="country")

# Definicja modelu City
class City(Base):
    __tablename__ = 'cities'
    city_id = Column(Integer, primary_key=True)
    city_name = Column(String)
    country_id = Column(Integer, ForeignKey('countries.country_id'))
    geolocation_id = Column(Integer, ForeignKey('geolocations.geolocation_id'))
    geolocation = relationship("Geolocation", back_populates="cities")
    country = relationship("Country", back_populates="cities")

# Funkcja zapisująca dane geolokalizacji do bazy
def create_geolocation(geometry_type, coordinates):
    """
    Creates a Geolocation record in the database, ensuring that invalid geometries are fixed.
    """
    # Convert coordinates to WKT format
    if geometry_type == 'Polygon':
        polygon_coords = ', '.join([f"{coord[0]} {coord[1]}" for coord in coordinates[0]])
        wkt_geometry = f"SRID=4326;POLYGON(({polygon_coords}))"
    elif geometry_type == 'MultiPolygon':
        multipolygon_coords = ', '.join([
            f"({', '.join([f'{coord[0]} {coord[1]}' for coord in polygon[0]])})"
            for polygon in coordinates
        ])
        wkt_geometry = f"SRID=4326;MULTIPOLYGON(({multipolygon_coords}))"
    elif geometry_type == 'Point':
        point_coords = f"{coordinates[0]} {coordinates[1]}"
        wkt_geometry = f"SRID=4326;POINT({point_coords})"
    else:
        raise ValueError(f"Unsupported geometry type: {geometry_type}")

    # Validate geometry before saving
    valid_geometry_query = func.ST_MakeValid(func.ST_SetSRID(func.ST_GeomFromEWKT(wkt_geometry), 4326))
    is_valid = session.query(func.ST_IsValid(valid_geometry_query)).scalar()

    if not is_valid:
        print(f"Warning: Geometry was invalid, fixing geometry.")
        wkt_geometry = session.query(valid_geometry_query).scalar()

    # Create geolocation record
    geolocation = Geolocation(area=wkt_geometry)
    session.add(geolocation)
    session.commit()
    return geolocation.geolocation_id



# Funkcja przetwarzająca kontynenty i zapisująca je do bazy
def process_continents(file_path):
    with open(file_path, 'r') as file:
        data = load_geojson(file)
    for feature in data['features']:
        geolocation_id = create_geolocation('Polygon', feature['geometry']['coordinates'])

        continent = Continent(
            continent_name=feature['properties']['continent_part'],
            geolocation_id=geolocation_id
        )
        session.add(continent)
    session.commit()

def coordinates_to_wkt(coordinates, geometry_type):
    """
    Converts coordinates from GeoJSON format to WKT (Well-Known Text) format.
    Handles both 'Polygon' and 'MultiPolygon'.
    """
    if geometry_type == 'Polygon':
        # Convert a single polygon
        rings = []
        for ring in coordinates:
            ring_coords = ', '.join([f"{point[0]} {point[1]}" for point in ring])
            rings.append(f"({ring_coords})")
        return f"POLYGON({', '.join(rings)})"  # Ensure no extra 'POLYGON' keyword

    elif geometry_type == 'MultiPolygon':
        # Convert a MultiPolygon
        polygons = []
        for polygon in coordinates:
            rings = []
            for ring in polygon:
                ring_coords = ', '.join([f"{point[0]} {point[1]}" for point in ring])
                rings.append(f"({ring_coords})")
            polygons.append(f"({', '.join(rings)})")
        return f"MULTIPOLYGON({', '.join(polygons)})"
    
    raise ValueError(f"Unsupported geometry type: {geometry_type}")



# Funkcja przetwarzająca kraje i przypisująca je do kontynentów na podstawie geolokalizacji
def process_countries(file_path):
    with open(file_path, 'r') as file:
        data = load_geojson(file)
    for feature in data['features']:
        geometry_type = feature['geometry']['type']
        coordinates = feature['geometry']['coordinates']

        # Konwertuj współrzędne do formatu WKT
        wkt_geometry = coordinates_to_wkt(coordinates, geometry_type)

        # Utwórz geolokalizację
        geolocation_id = create_geolocation(geometry_type, coordinates)

        # Upewnij się, że geometria jest poprawna, użyj ST_MakeValid, jeśli nie
        valid_geometry_query = func.ST_MakeValid(func.ST_SetSRID(func.ST_GeomFromEWKT(f'SRID=4326;{wkt_geometry}'), 4326))
        is_valid = session.query(func.ST_IsValid(valid_geometry_query)).scalar()

        if not is_valid:
            print(f"Warning: Geometry for {feature['properties']['ADMIN']} is invalid and was fixed.")
            wkt_geometry = session.query(valid_geometry_query).scalar()

        # Znajdź kontynent, który zawiera dany kraj na podstawie geolokalizacji
        continent = session.query(Continent).join(Geolocation).filter(
            func.ST_Contains(Geolocation.area, valid_geometry_query)
        ).first()

        # Ręczne przypisanie kontynentu dla Antarktydy
        if feature['properties']['ADMIN'] == 'Antarctica':
            continent = session.query(Continent).filter_by(continent_name='Antarctica').first()

        if not continent:
            print(f"Warning: No continent found for {feature['properties']['ADMIN']}")
            continue  # Pomijanie kraju, jeśli nie znaleziono kontynentu

        # Tworzenie rekordu dla kraju
        country = Country(
            country_name=feature['properties']['ADMIN'],
            geolocation_id=geolocation_id,
            continent_id=continent.continent_id
        )
        session.add(country)
    session.commit()


# Funkcja przetwarzająca miasta i przypisująca je do krajów na podstawie geolokalizacji
def process_cities(file_path):
    """
    Processes city data from a GeoJSON file, converts coordinates to WKT, 
    validates geometries, and assigns cities to countries based on geolocation.
    """
    with open(file_path, 'r') as file:
        data = load_geojson(file)

    for feature in data['features']:
        # Check if geometry exists and has coordinates
        if 'geometry' not in feature or 'coordinates' not in feature['geometry']:
            print(f"Warning: Missing geometry or coordinates for city: {feature.get('properties', {}).get('NAME', 'Unknown')}")
            continue

        geometry_type = feature['geometry']['type']
        coordinates = feature['geometry']['coordinates']

        # Check if coordinates are valid
        if not coordinates:
            print(f"Warning: Invalid coordinates for city: {feature.get('properties', {}).get('NAME', 'Unknown')}")
            continue

        # Process based on geometry type
        if geometry_type == 'Polygon':
            geolocation_id = create_geolocation('Polygon', coordinates)
        else:
            print(f"Warning: Unsupported geometry type {geometry_type} for city: {feature['properties']['NAME']}")
            continue

        # Convert coordinates to WKT for ST_Contains query
        wkt_polygon = coordinates_to_wkt(coordinates, 'Polygon')

        # Validate city geometry
        valid_geometry_query = func.ST_MakeValid(func.ST_SetSRID(func.ST_GeomFromText(wkt_polygon), 4326))
        is_valid = session.query(func.ST_IsValid(valid_geometry_query)).scalar()

        if not is_valid:
            print(f"Warning: Geometry for city {feature['properties']['NAME']} is invalid and was fixed.")
            wkt_polygon = session.query(valid_geometry_query).scalar()

        # Find country containing the city based on the geolocation
        country = session.query(Country).join(Geolocation).filter(
            func.ST_Contains(Geolocation.area, valid_geometry_query)
        ).first()

        if not country:
            print(f"Warning: No country found for city: {feature['properties']['NAME']}")
            continue

        # Create city record
        city = City(
            city_name=feature['properties']['NAME'],
            geolocation_id=geolocation_id,
            country_id=country.country_id
        )
        session.add(city)

    session.commit()



# Wczytaj dane GeoJSON i przetwórz do bazy danych
process_continents('GeolocationsGeoJSONs/continent.geojson')
process_countries('GeolocationsGeoJSONs/countries.geojson')
process_cities('GeolocationsGeoJSONs/cities.geojson')

print("Dane zostały przetworzone i zapisane z uwzględnieniem tabeli Geolocation.")

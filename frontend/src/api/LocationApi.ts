import { api } from './axios';

export const getContinents = async () => {
    try {
        const response = await api.get('Continent');
        console.log(response.data);
        return response.data.continents.map((continent: { continentId: number; continentName: string; geolocationIdFk: number }) => ({
            id: continent.continentId,
            name: continent.continentName,
            geolocation: continent.geolocationIdFk 
        }));
    } catch (error) {
        throw new Error('Failed to fetch continents.');
    }
};

export const getCountries = async (continentId: number) => {
    try {
        const response = await api.get(`Country/by-continent/${continentId}`);
        console.log(response.data);
        return response.data.countries.map((country: { countryId: number; countryName: string; geolocationIdFk: number  }) => ({
            id: country.countryId,
            name: country.countryName,
            geolocation: country.geolocationIdFk
        }));
    } catch (error) {
        throw new Error('Failed to fetch countries.');
    }
};

export const getCities = async (countryId: number) => {
    try {
        const response = await api.get(`City/by-country/${countryId}`);
        return response.data.cities.map((city: { cityId: number; cityName: string; geolocationIdFk: number }) => ({
            id: city.cityId,
            name: city.cityName,
            geolocation: city.geolocationIdFk
        }));
    } catch (error) {
        throw new Error('Failed to fetch cities.');
    }
};

export const getGeolocation = async (geolocationId: number) => {
    try {
        const response = await api.get(`Geolocation/${geolocationId}`);
        return response.data;
    } catch (error) {
        throw new Error('Failed to fetch geolocation.');
    }
};

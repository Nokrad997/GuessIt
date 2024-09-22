import { useState } from 'react';
import { getContinents, getCountries, getCities, getGeolocation } from '../api/LocationApi';

const useLocation = () => {
    const [continents, setContinents] = useState<{ id: number; name: string, geolocation: number; }[]>([]);
    const [countries, setCountries] = useState<{ id: number; name: string, geolocation: number; }[]>([]);
    const [geolocation, setGeolocation] = useState<{ geolocationId: number; area: any } | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

    const fetchContinents = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await getContinents();
            console.log(data);
            setContinents(data);
        } catch (err) {
            setError('Failed to fetch continents.');
        } finally {
            setLoading(false);
        }
    };

    const fetchCountries = async (continentId: number) => {
        setLoading(true);
        setError(null);
        try {
            const data = await getCountries(continentId);
            setCountries(data);
        } catch (err) {
            setError('Failed to fetch countries.');
        } finally {
            setLoading(false);
        }
    };

    const fetchGeolocation = async (geolocationId: number) => {
        setLoading(true);
        setError(null);
        try {
            const data = await getGeolocation(geolocationId);
            console.log(data)
            setGeolocation(data.geolocation);
            sleep(5000)
        } catch (err) {
            setError('Failed to fetch geolocation.');
        } finally {
            setLoading(false);
        }
    };

    return {
        continents,
        countries,
        geolocation,
        loading,
        error,
        fetchContinents,
        fetchCountries,
        fetchGeolocation,
    };
};

export default useLocation;

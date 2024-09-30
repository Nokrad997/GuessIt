import { useState } from 'react';

interface LatLng {
	lat: number;
	lng: number;
}

const useGame = () => {
    const [generatedLocation, setGeneratedLocation] = useState<LatLng | null>(null);
    const [selectedLocation, setSelectedLocation] = useState<LatLng | null>(null);
    const [guessedLocation, setGuessedLocation] = useState<LatLng | null>(null);
    const [panoramaFound, setPanoramaFound] = useState(false);
    const [distance, setDistance] = useState(0);
    const [score, setScore] = useState(0);
    const [startTime, setStartTime] = useState<Date | null>(null);
    const [endTime, setEndTime] = useState<Date | null>(null);
    
    return {
        generatedLocation,
        setGeneratedLocation,
        selectedLocation,
        setSelectedLocation,
        guessedLocation,
        setGuessedLocation,
        panoramaFound,
        setPanoramaFound,
        distance,
        setDistance,
        score,
        setScore,
        startTime,
        setStartTime,
        endTime,
        setEndTime,
    };
};

export default useGame;

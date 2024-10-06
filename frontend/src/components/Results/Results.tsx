import React, { useRef, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Polyline, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import useGame from '../../hooks/useGame';

interface Location {
  lat: number;
  lng: number;
}

interface ResultsProps {
  score: number;
  distance: number;
  startTime: Date;
  endTime: Date;
  timeElapsed: number;
  selectedLocation: Location;
  guessedLocation: Location;
  gameType: string;
}

const Results: React.FC<ResultsProps> = ({ score, distance, startTime, endTime, timeElapsed, selectedLocation, guessedLocation, gameType }) => {
  const { sendGameResults } = useGame();

  useEffect(() => {
    const passGameResults = async () => {
      await sendGameResults({
        StartLocation: {
          type: 'Point',
          coordinates: [selectedLocation.lat, selectedLocation.lng]
        },
        GuessedLocation: {
          type: 'Point',
          coordinates: [guessedLocation.lat, guessedLocation.lng]
        },
        DistanceToStartingLocation: distance,
        StartTime: startTime,
        EndTime: endTime
      }, gameType)
    };

    passGameResults();
  }, []);

  return (
    <div>
      <h1>Score: {score} points</h1>
      <p>Distance: {distance.toFixed(2)} km</p>
      <p>Time: {timeElapsed.toFixed(2)} s</p>
      <MapContainer
        center={[selectedLocation.lat, selectedLocation.lng]}
        zoom={2}
        style={{ height: '400px', width: '100%' }}
      >
        <TileLayer
          // url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          // attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png?lang=en"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />

        <Marker position={[selectedLocation.lat, selectedLocation.lng]}>
          <Popup>
            Selected Location
          </Popup>
        </Marker>

        <Marker position={[guessedLocation.lat, guessedLocation.lng]}>
          <Popup>
            Guessed Location
          </Popup>
        </Marker>

        <Polyline positions={[
          [selectedLocation.lat, selectedLocation.lng],
          [guessedLocation.lat, guessedLocation.lng]
        ]} color="blue" />
      </MapContainer>
    </div>
  );
};

export default Results;

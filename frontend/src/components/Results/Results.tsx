import React, { useRef, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Polyline, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import useGame from '../../hooks/useGame';
import { Card, Container } from 'react-bootstrap';

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
  traveledDistance: number;
  guessedLocation: Location;
  gameType: string;
}

const Results: React.FC<ResultsProps> = ({ score, distance, startTime, endTime, timeElapsed, selectedLocation, traveledDistance, guessedLocation, gameType }) => {
  const { sendGameResults } = useGame();

  useEffect(() => {
    console.log(traveledDistance)
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
        TraveledDistance: traveledDistance,
        StartTime: startTime,
        EndTime: endTime
      }, gameType)
    };

    passGameResults();
  }, []);

  return (
    <div
      style={{
        minHeight: '100vh',
        background: 'linear-gradient(6deg, rgba(2,0,36,1) 0%, rgba(27,61,134,1) 35%, rgba(0,212,255,1) 100%)',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        color: 'white',
        padding: '20px'
      }}
    >
      <Card
        style={{ width: '95%'}}
      >
        <Card.Body>
          <h1>Score: {score} points</h1>
          <p>Distance: {distance.toFixed(2)} km</p>
          <p>Time: {timeElapsed.toFixed(2)} s</p>

          <MapContainer
            center={[selectedLocation.lat, selectedLocation.lng]}
            zoom={2}
            style={{ height: '400px', width: '100%' }}
          >
            <TileLayer
              url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            // url="https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png?lang=en"
            // attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
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
        </Card.Body>
      </Card>
    </div>
  );
};

export default Results;

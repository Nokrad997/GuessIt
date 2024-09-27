import React, { useRef, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Polyline, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';

interface Location {
  lat: number;
  lng: number;
}

interface ResultsProps {
  score: number;
  distance: number;
  timeElapsed: number;
  selectedLocation: Location;
  guessedLocation: Location;
}

const Results: React.FC<ResultsProps> = ({ score, distance, timeElapsed, selectedLocation, guessedLocation }) => {
  const selectedMarkerRef = useRef<any>(null);
  const guessedMarkerRef = useRef<any>(null);

  useEffect(() => {
    if (selectedMarkerRef.current) {
      selectedMarkerRef.current.openPopup();
    }
    if (guessedMarkerRef.current) {
      guessedMarkerRef.current.openPopup();
    }
  }, [selectedLocation, guessedLocation]);

  return (
    <div>
      <h1>Wynik: {score} punktów</h1>
      <p>Dystans: {distance} km</p>
      <p>Czas: {timeElapsed.toFixed(2)} sekund</p>
      <MapContainer
        center={[selectedLocation.lat, selectedLocation.lng]}
        zoom={2}
        style={{ height: '400px', width: '100%' }}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />

        <Marker position={[selectedLocation.lat, selectedLocation.lng]} ref={selectedMarkerRef}>
          <Popup>
            Selected Location
          </Popup>
        </Marker>

        <Marker position={[guessedLocation.lat, guessedLocation.lng]} ref={guessedMarkerRef}>
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

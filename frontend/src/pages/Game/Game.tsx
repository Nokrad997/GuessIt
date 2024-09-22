
import useLocation from '../../hooks/useLocation';
import { useLocation as useRouterLocation } from 'react-router-dom';
import React, { useState, useEffect, useRef } from 'react';
import { StreetViewPanorama, useJsApiLoader } from '@react-google-maps/api';
import { MapContainer, TileLayer, useMapEvents, useMap } from 'react-leaflet';
import { Container, Button, Modal } from 'react-bootstrap';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import './Game.css';
import * as turf from '@turf/turf';

interface LatLng {
	lat: number;
	lng: number;
}

interface minMaxOfGeolocation {
	minLat: number;
	maxLat: number;
	minLng: number;
	maxLng: number;
}

const Game = () => {
	const panoramaRef = useRef<HTMLDivElement | null>(null);
	const [showMap, setShowMap] = useState(false);
	const [showModal, setShowModal] = useState(false);
	const [selectedLocation, setSelectedLocation] = useState<LatLng | null>(null);
	const { fetchGeolocation, geolocation } = useLocation();
	const mapRef = useRef(null);
	const routerLocation = useRouterLocation();
	const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

	useEffect(() => {
		const queryParams = new URLSearchParams(routerLocation.search);
		const geolocationId = queryParams.get('geolocationId');
		const startGame = async () => {
			if (geolocationId) {
				await fetchGeolocation(parseInt(geolocationId))
				console.log(geolocation);
				findLatAndLngFromGeolocation();
				if (selectedLocation != null) {
					generateRandomPoint();
				}
			}
		}

		startGame();
	}, []);

	const getMinMaxOfGeolocation = (): minMaxOfGeolocation => {
		const coordinates = geolocation!.area.coordinates[0][0];
		let minLat = 90;
		let maxLat = -90;
		let minLng = 180;
		let maxLng = -180;

		coordinates.forEach(([lng, lat]: [number, number]) => {
			minLat = Math.min(minLat, lat);
			maxLat = Math.max(maxLat, lat);
			minLng = Math.min(minLng, lng);
			maxLng = Math.max(maxLng, lng);
		});

		return { minLat, maxLat, minLng, maxLng };
	}

	const findLatAndLngFromGeolocation = () => {
		if (geolocation) {
			const coordinates = geolocation.area.coordinates[0][0];
	
			// Zdefiniuj wielokąt Polski
			const polygon = turf.polygon([coordinates]);
			const { minLat, maxLat, minLng, maxLng } = getMinMaxOfGeolocation();
			console.log(minLat, maxLat, minLng, maxLng);
			let randomPoint = null;
			let isInside = false;
	
			// Generuj losowe punkty aż znajdziesz taki, który jest wewnątrz wielokąta
			while (!isInside) {
				// Losuj szerokość i długość geograficzną w granicach Polski
				const randomLat = Math.random() * (maxLat - minLat) + minLat;
				const randomLng = Math.random() * (maxLng - minLng) + minLng;
	
				// Stwórz punkt z losowymi współrzędnymi
				randomPoint = turf.point([randomLng, randomLat]);
	
				// Sprawdź, czy punkt jest wewnątrz wielokąta
				isInside = turf.booleanPointInPolygon(randomPoint, polygon);
				console.log(isInside);
			}
	
			// Jeśli punkt jest wewnątrz, ustaw jego współrzędne
			setSelectedLocation({
				lat: randomPoint!.geometry.coordinates[1],
				lng: randomPoint!.geometry.coordinates[0],
			});
			sleep(5000);
			console.log(selectedLocation);
		}
	};

	const generateRandomPoint = () => {
		const sv = new window.google.maps.StreetViewService();
		console.log(sv);
		if(selectedLocation != null){
			const randomPoint = new window.google.maps.LatLng(selectedLocation.lat, selectedLocation.lng);
			console.log(randomPoint);

			sv.getPanorama({ location: randomPoint, radius: 2000 }, processSVData);
		}
	};

	// Define the parameter types for processSVData
	const processSVData = (data: google.maps.StreetViewPanoramaData | null, status: google.maps.StreetViewStatus) => {
		if (status === window.google.maps.StreetViewStatus.OK && panoramaRef.current && data?.location?.latLng) {
			const panorama = new window.google.maps.StreetViewPanorama(
				panoramaRef.current, // Use the ref to get the panorama container
				{
					position: data.location.latLng,
					pov: {
						heading: 34,
						pitch: 10,
					},
					disableDefaultUI: true,
				}
			);
		} else {
			// Retry with a new random point if no panorama is found
			generateRandomPoint();
		}
	};

	const handleMapClick = (event: any) => {
		setSelectedLocation(event.latlng);
		setShowModal(true);
	};

	const handleConfirmLocation = () => {
		setShowModal(false);
		setShowMap(false);
	};

	const MapClickHandler = () => {
		useMapEvents({
			click: handleMapClick,
		});
		return null;
	};

	const RestrictMapMovement = () => {
		const map = useMap();

		useEffect(() => {
			if (map) {
				const bounds = L.latLngBounds(
					L.latLng(-85, -180),
					L.latLng(85, 180)
				);
				map.setMaxBounds(bounds);
			}
		}, [map]);

		return null;
	};

	return (
		<div id="panoramamap" ref={panoramaRef} style={{ width: '100%', height: '100vh' }}>
			{/* The panorama will be rendered inside this div */}
			<div className={`map-container-animated ${showMap ? 'show' : ''}`}>
				<MapContainer
					center={[20, 0]}
					zoom={2}
					style={{ height: '100%', width: '100%' }}
					ref={mapRef}
				>
					<TileLayer
						url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
						attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
					/>
					<MapClickHandler />
					<RestrictMapMovement />
				</MapContainer>

				<Button
					onClick={() => setShowMap(prev => !prev)}
					className="vertical-button"
					variant="primary"
				>
					Toggle World Map
				</Button>
			</div>

			<Modal show={showModal} onHide={() => setShowModal(false)}>
				<Modal.Header closeButton>
					<Modal.Title>Confirm Location</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					Are you sure you want to select this location?
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={() => setShowModal(false)}>
						Cancel
					</Button>
					<Button variant="primary" onClick={handleConfirmLocation}>
						Yes
					</Button>
				</Modal.Footer>
			</Modal>
		</div>
	);
};

export default Game;




